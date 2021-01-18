using DotDll.Model.Data.Members;

namespace DotDll.Presentation.Model
{
    public class ConstructorNode : MemberNode<Constructor>
    {
        public ConstructorNode(Constructor constructor) : base(constructor)
        {
            Name = $"{GetAttributesString(constructor)}{GetAccessString(constructor.AccessLevel)} " + 
                   $"{constructor.Name}{MapParameters(constructor.Parameters)}";
        }

        public override string Name { get; }
    }
}