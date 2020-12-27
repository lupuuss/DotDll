using System;
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
        public MetaDataDeclarations Map(DllInfo dllInfo)
        {
            var namespaces = dllInfo.Namespaces
                .Select(MapNamespace)
                .ToList();

            return new MetaDataDeclarations(dllInfo.Name, namespaces);
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
            if (type.TypeKind is Type.Kind.GenericArg) return new DType(type.Name);

            var name = GetAccessString(type.Access);

            if (type.IsStatic)
            {
                name += " static";
            }
            else
            {
                if (type.IsSealed) name += " sealed";

                if (type.IsAbstract) name += " abstract";
            }

            name += " ";

            switch (type.TypeKind)
            {
                case Type.Kind.Interface:
                    name += "interface";
                    break;
                case Type.Kind.Class:
                    name += "class";
                    break;
                case Type.Kind.Enum:
                    name += "enum";
                    break;
                case Type.Kind.Array:
                    name += "[]";
                    break;
                case Type.Kind.GenericArg:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            name += " " + type.Name;

            if (!type.GenericArguments.Any()) return new DType(name, type.Members.Select(MapMember).ToList());

            name += "<";
            name += string.Join(", ", type.GenericArguments.Select(arg => arg.Name));
            name += ">";

            return new DType(name, type.Members.Select(MapMember).ToList());
        }

        private DMember MapMember(Member member)
        {
            switch (member)
            {
                case Event eve:
                    return MapEvent(eve);
                case Constructor constructor:
                    return MapConstructor(constructor);
                case Field field:
                    return MapField(field);
                case Method method:
                    return MapMethod(method);
                case NestedType nestedType:
                    return MapNestedType(nestedType);
                case Property property:
                    return MapProperty(property);
                default:
                    throw new ArgumentOutOfRangeException(nameof(member));
            }
        }

        private DMember MapProperty(Property property)
        {
            throw new NotImplementedException();
        }

        private DMember MapNestedType(NestedType nestedType)
        {
            throw new NotImplementedException();
        }

        private DMember MapMethod(Method method)
        {
            throw new NotImplementedException();
        }

        private DMember MapField(Field field)
        {
            throw new NotImplementedException();
        }

        private DMember MapConstructor(Constructor constructor)
        {
            throw new NotImplementedException();
        }

        private DMember MapEvent(Event eve)
        {
            throw new NotImplementedException();
        }

        private string GetAccessString(Access typeAccess)
        {
            switch (typeAccess)
            {
                case Access.Public:
                    return "public";
                case Access.Internal:
                    return "public";
                case Access.Protected:
                    return "protected";
                case Access.InternalProtected:
                    return "internal protected";
                case Access.Private:
                    return "private";
                case Access.Inner:
                    return "";
                default:
                    throw new ArgumentOutOfRangeException(nameof(typeAccess), typeAccess, null);
            }
        }
    }
}