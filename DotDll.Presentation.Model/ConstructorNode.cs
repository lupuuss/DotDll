using DotDll.Model.Data.Members;

namespace DotDll.Presentation.Model
{
    public class ConstructorNode : MemberNode<Constructor>
    {
        private readonly Constructor _constructor;

        public ConstructorNode(Constructor constructor) : base(constructor)
        {
            _constructor = constructor;
            Name = $"{GetAttributesString(constructor)}{GetAccessString(constructor.AccessLevel)} " + 
                   $"{constructor.Name}{MapParameters(constructor.Parameters)}";
        }

        public override string Name { get; }
    }
}