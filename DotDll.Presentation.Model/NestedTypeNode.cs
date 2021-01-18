using DotDll.Model.Data.Members;

namespace DotDll.Presentation.Model
{
    public class NestedTypeNode : MemberNode<NestedType>
    {
        public NestedTypeNode(NestedType nestedType) : base(nestedType)
        {
            Name = BuildName();
        }

        private string BuildName()
        {
            var declaration = $"{GetAttributesString(Member)}(nested) ";

            declaration +=  new TypeNode(Member.Type).Name;

            return declaration;
        }

        public override string Name { get; }
    }
}