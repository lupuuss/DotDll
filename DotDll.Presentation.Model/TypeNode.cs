using System.Linq;
using DotDll.Model.Data;
using DotDll.Model.Data.Base;
using DotDll.Model.Data.Members;

namespace DotDll.Presentation.Model
{
    public class TypeNode : MetadataNode
    {
        private readonly Type _type;

        public TypeNode(Type type)
        {
            _type = type;
            Name = BuildName();
        }

        private string BuildName()
        {

            if (_type.TypeKind is Type.Kind.GenericArg) return _type.Name;

            string declaration = "";

            if (_type.Attributes.Any()) declaration = $"[{string.Join(", ", _type.Attributes.Select(a => a.Name))}]\n";

            declaration += GetAccessString(_type.Access);

            if (_type.IsStatic)
            {
                declaration += " static";
            }
            else
            {
                if (_type.TypeKind != Type.Kind.Enum && _type.IsSealed) declaration += " sealed";

                if (_type.TypeKind != Type.Kind.Interface && _type.IsAbstract) declaration += " abstract";
            }

            declaration += $" {MapTypeKind(_type.TypeKind)} {_type.FullName()}";

            if (_type.BaseTypes.Any())
                declaration += $" : {string.Join(", ", _type.BaseTypes.Select(t => t.FullName()))}";

            return declaration;
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
                _ => throw new System.ArgumentOutOfRangeException()
            };
        }

        public override string Name { get; }
        public override void LoadChildren()
        {
            foreach (var member in _type.Members)
            {
                var node = BuildMemberNode(member);
                Nodes.Add(node);
            }
        }

        private MetadataNode BuildMemberNode(Member member)
        {
            return member switch
            {
                Event eve => new EventNode(eve),
                Constructor constructor => new ConstructorNode(constructor),
                Field field => new FieldNode(field),
                Method method => new MethodNode(method),
                NestedType nestedType => new NestedTypeNode(nestedType),
                Property property => new PropertyNode(property),
                _ => throw new System.ArgumentOutOfRangeException(nameof(member))
            };
        }
    }
}