using DotDll.Model.Data.Members;

namespace DotDll.Presentation.Model
{
    public class EventNode : MemberNode<Event>
    {
        public EventNode(Event eve) : base(eve)
        {
            Name = BuildName();
        }

        private string BuildName()
        {
            var declaration = $"{GetAttributesString(Member)}(event) {Member.EventType} {Member.Name} {{";

            if (Member.AddMethod != null) declaration += $" {GetAccessString(Member.AddMethod.AccessLevel)} add; ";

            if (Member.RemoveMethod != null) declaration += $"{GetAccessString(Member.RemoveMethod.AccessLevel)} remove; ";

            if (Member.RaiseMethod != null) declaration += $"{GetAccessString(Member.RaiseMethod.AccessLevel)} raise; ";

            declaration += "}";

            return declaration;
        }

        public override string Name { get; }
    }
}