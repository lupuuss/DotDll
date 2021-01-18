using DotDll.Model.Data.Members;

namespace DotDll.Presentation.Model
{
    public class PropertyNode : MemberNode<Property>
    {
        public PropertyNode(Property property) : base(property)
        {
            Name = BuildName();
        }

        private string BuildName()
        {
            var declaration = $"{GetAttributesString(Member)}(property) {Member.ReturnType.FullName()} {Member.Name} ";

            if (Member.CanRead) declaration += $"{{ {GetAccessString(Member.Getter!.AccessLevel)} get; ";

            if (Member.CanWrite) declaration += $"{GetAccessString(Member.Setter!.AccessLevel)} set; ";

            if (Member.CanRead) declaration += "}";

            return declaration;
        }

        public override string Name { get; }
    }
}