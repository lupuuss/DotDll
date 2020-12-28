using System;
using System.Collections.Generic;
using System.Linq;
using DotDll.Logic.Metadata.Data;
using DotDll.Model.Data;
using DotDll.Model.Data.Base;
using DotDll.Model.Data.Members;
using Type = DotDll.Model.Data.Type;

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

            var declaration = GetAccessString(type.Access);

            if (type.IsStatic)
            {
                declaration += " static";
            }
            else
            {
                if (type.IsSealed) declaration += " sealed";

                if (type.IsAbstract) declaration += " abstract";
            }

            declaration += " ";

            switch (type.TypeKind)
            {
                case Type.Kind.Interface:
                    declaration += "interface";
                    break;
                case Type.Kind.Class:
                    declaration += "class";
                    break;
                case Type.Kind.Enum:
                    declaration += "enum";
                    break;
                case Type.Kind.Array:
                    declaration += "[]";
                    break;
                case Type.Kind.GenericArg:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            declaration += " " + type.Name;

            if (type.GenericArguments.Any())
            {
                declaration += "<";
                declaration += string.Join(", ", type.GenericArguments.Select(arg => arg.Name));
                declaration += ">";
            }

            var dType = new DType(declaration);

            _typesMapping[type] = dType;

            foreach (var member in type.Members) dType.Members.Add(MapMember(member));

            return dType;
        }

        private DMember MapMember(Member member)
        {
            return member switch
            {
                Event eve => MapEvent(eve),
                Constructor constructor => MapConstructor(constructor),
                Field field => MapField(field),
                Method method => MapMethod(method),
                NestedType nestedType => MapNestedType(nestedType),
                Property property => MapProperty(property),
                _ => throw new ArgumentOutOfRangeException(nameof(member))
            };
        }

        private DMember MapProperty(Property property)
        {
            var declaration = $"(property) {property.ReturnType.Name} {property.Name}";

            if (property.CanRead) declaration += $"{{ {GetAccessString(property.AccessLevel)} get; ";

            if (property.CanWrite) declaration += $"{GetAccessString(property.AccessLevel)} set; ";

            if (property.CanRead) declaration += "}";

            return new DMember(declaration, property.GetRelatedTypes().Select(MapType).ToList());
        }

        private DMember MapNestedType(NestedType nestedType)
        {
            var declaration = "(nested) ";
            var dType = MapType(nestedType.Type);

            declaration += dType.Declaration;

            return new DMember(declaration, new List<DType> {dType});
        }

        private DMember MapMethod(Method method)
        {
            var declaration = GetAccessString(method.AccessLevel) + " ";

            if (method.IsStatic)
                declaration += "static ";
            else if (method.IsAbstract)
                declaration += "abstract ";

            declaration += $"{method.ReturnType.Name} {method.Name}";

            declaration += MapParameters(method.Parameters);

            return new DMember(declaration, method.GetRelatedTypes().Select(MapType).ToList());
        }

        private DMember MapField(Field field)
        {
            var constraint = field.FieldConstraint switch
            {
                Field.Constraint.None => "",
                Field.Constraint.ReadOnly => "readonly",
                Field.Constraint.Const => "const",
                _ => throw new ArgumentOutOfRangeException()
            };

            var declaration = constraint != ""
                ? $"{GetAccessString(field.AccessLevel)} {constraint} "
                : $"{GetAccessString(field.AccessLevel)} ";


            declaration += $"{field.ReturnType.Name} {field.Name}";

            return new DMember(declaration, field.GetRelatedTypes().Select(MapType).ToList());
        }

        private DMember MapConstructor(Constructor constructor)
        {
            return MapMethod(constructor);
        }

        private string MapParameters(IEnumerable<Parameter> parameters)
        {
            return "(" + string.Join(
                ", ",
                parameters.Select(param => $"{param.ParameterType.Name} {param.Name}")
            ) + ")";
        }

        private DMember MapEvent(Event eve)
        {
            var declaration = $"(event) {eve.AddMethod.ReturnType.Name} {eve.Name}";

            declaration += $"{{ {GetAccessString(eve.AddMethod.AccessLevel)} add; ";

            declaration += $"{GetAccessString(eve.RemoveMethod.AccessLevel)} remove; ";

            declaration += $"{GetAccessString(eve.RaiseMethod.AccessLevel)} raise; }}";

            return new DMember(declaration, eve.GetRelatedTypes().Select(MapType).ToList());
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
    }
}