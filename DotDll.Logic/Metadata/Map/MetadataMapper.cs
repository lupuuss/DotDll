﻿using System;
using System.Collections.Generic;
using System.Linq;
using DotDll.Logic.Metadata.Data;
using DotDll.Model.Data;
using DotDll.Model.Data.Base;
using DotDll.Model.Data.Members;
using Type = DotDll.Model.Data.Type;

// ReSharper disable MemberCanBeMadeStatic.Local

namespace DotDll.Logic.Metadata.Map
{
    public class MetadataMapper : IMetadataMapper
    {
        private readonly Dictionary<Type, DType> _typesMapping = new Dictionary<Type, DType>();

        public MetadataDeclarations Map(MetadataInfo metadataInfo)
        {
            _typesMapping.Clear();

            var namespaces = metadataInfo.Namespaces
                .Select(MapNamespace)
                .ToList();

            return new MetadataDeclarations(metadataInfo.Name, namespaces);
        }

        private DNamespace MapNamespace(Namespace nSpace)
        {
            var types = nSpace.Types
                .Select(MapType)
                .ToList();

            return new DNamespace(nSpace.Name, types);
        }

        private DType MapType(Type type)
        {
            if (_typesMapping.ContainsKey(type)) return _typesMapping[type];

            if (type.TypeKind is Type.Kind.GenericArg) return new DType(type.Name);

            string declaration = "";

            if (type.Attributes.Any())
            {
                declaration = $"[{string.Join(", ", type.Attributes.Select(a => a.Name))}]\n";
            }
            
            declaration += GetAccessString(type.Access);

            if (type.IsStatic)
            {
                declaration += " static";
            }
            else
            {
                if (type.TypeKind != Type.Kind.Enum && type.IsSealed) declaration += " sealed";

                if (type.TypeKind != Type.Kind.Interface && type.IsAbstract) declaration += " abstract";
            }

            declaration += $" {MapTypeKind(type.TypeKind)} {type.FullName()}";

            if (type.BaseTypes.Any())
            {
                declaration += $" : {string.Join(", ", type.BaseTypes.Select(t => t.FullName()))}";
            }
            
            var dType = new DType(declaration);

            _typesMapping[type] = dType;

            foreach (var member in type.Members) dType.Members.Add(MapMember(member));

            return dType;
        }

        private string MapTypeKind(Type.Kind kind)
        {
            return kind switch
            {
                Type.Kind.Interface => "interface",
                Type.Kind.Class => "class",
                Type.Kind.Enum => "enum",
                Type.Kind.Array => "[]",
                Type.Kind.GenericArg => "",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private DMember MapMember(Member member)
        {

            var declarationInit = GetAttributesString(member);
            
            return member switch
            {
                Event eve => MapEvent(eve, declarationInit),
                Constructor constructor => MapConstructor(constructor, declarationInit),
                Field field => MapField(field, declarationInit),
                Method method => MapMethod(method, declarationInit),
                NestedType nestedType => MapNestedType(nestedType, declarationInit),
                Property property => MapProperty(property, declarationInit),
                _ => throw new ArgumentOutOfRangeException(nameof(member))
            };
        }

        private DMember MapProperty(Property property, string declarationInit)
        {
            var declaration = $"{declarationInit}(property) {property.ReturnType.FullName()} {property.Name} ";

            if (property.CanRead) declaration += $"{{ {GetAccessString(property.Getter!.AccessLevel)} get; ";

            if (property.CanWrite) declaration += $"{GetAccessString(property.Setter!.AccessLevel)} set; ";

            if (property.CanRead) declaration += "}";

            return new DMember(declaration, property.GetRelatedTypes().Select(MapType).ToList());
        }

        private DMember MapNestedType(NestedType nestedType, string declarationInit)
        {
            var declaration = $"{declarationInit}(nested) ";
            var dType = MapType(nestedType.Type);

            declaration += dType.Declaration;

            return new DMember(declaration, new List<DType> {dType});
        }

        private DMember MapMethod(Method method, string declarationInit)
        {
            var declaration = $"{declarationInit}{GetAccessString(method.AccessLevel)} ";

            if (method.IsStatic)
                declaration += "static ";
            else if (method.IsAbstract)
                declaration += "abstract ";
            else if (method.IsVirtual) 
                declaration += "virtual ";

            if (method.IsSealed) declaration += "sealed ";

            declaration += $"{method.ReturnType.FullName()} {method.Name}";

            if (method.GenericArguments.Any())
                declaration += MapGenericArguments(method.GenericArguments);

            declaration += MapParameters(method.Parameters);

            return new DMember(declaration, method.GetRelatedTypes().Select(MapType).ToList());
        }

        private string MapGenericArguments(IEnumerable<Type> methodGenericArguments)
        {

            var args = string.Join(", ", methodGenericArguments.Select(arg => arg.FullName()));
            
            return $"<{args}>";
        }

        private DMember MapField(Field field, string declarationInit)
        {
            var constraint = field.FieldConstraint switch
            {
                Field.Constraint.None => "",
                Field.Constraint.ReadOnly => "readonly",
                Field.Constraint.Const => "const",
                _ => throw new ArgumentOutOfRangeException()
            };

            var declaration = declarationInit;
            
            declaration += constraint != ""
                ? $"{GetAccessString(field.AccessLevel)} {constraint} "
                : $"{GetAccessString(field.AccessLevel)} ";


            declaration += $"{field.ReturnType.FullName()} {field.Name}";

            return new DMember(declaration, field.GetRelatedTypes().Select(MapType).ToList());
        }

        private DMember MapConstructor(Constructor constructor, string declarationInit)
        {
            var declaration = $"{declarationInit}{GetAccessString(constructor.AccessLevel)} " +
                              $"{constructor.Name}{MapParameters(constructor.Parameters)}";

            return new DMember(declaration, constructor.GetRelatedTypes().Select(MapType).ToList());
        }

        private DMember MapEvent(Event eve, string declarationInit)
        {
            var declaration = $"{declarationInit}(event) {eve.EventType} {eve.Name} {{";

            if (eve.AddMethod != null) declaration += $" {GetAccessString(eve.AddMethod.AccessLevel)} add; ";

            if (eve.RemoveMethod != null) declaration += $"{GetAccessString(eve.RemoveMethod.AccessLevel)} remove; ";

            if (eve.RaiseMethod != null) declaration += $"{GetAccessString(eve.RaiseMethod.AccessLevel)} raise; ";

            declaration += "}";

            return new DMember(declaration, eve.GetRelatedTypes().Select(MapType).ToList());
        }
        
        private string MapParameters(IEnumerable<Parameter> parameters)
        {
            return "(" + string.Join(
                ", ",
                parameters.Select(param =>
                {
                    var attrs = param.Attributes.Any()
                        ? "[" + string.Join(",", param.Attributes.Select(a => a.Name)) + "]"
                        : "";
                    
                    return $"{attrs} {param.ParameterType.FullName()} {param.Name}";
                })
            ) + ")";
        }
        
        private string GetAccessString(Access typeAccess)
        {
            return typeAccess switch
            {
                Access.Public => "public",
                Access.Internal => "public",
                Access.Protected => "protected",
                Access.InternalProtected => "internal protected",
                Access.Private => "private",
                Access.Inner => "",
                _ => throw new ArgumentOutOfRangeException(nameof(typeAccess), typeAccess, null)
            };
        }

        private string GetAttributesString(Member member)
        {
            return member.Attributes.Any() ? $"[{string.Join(", ", member.Attributes.Select(a => a.Name))}]\n" : "";
        }
    }
}