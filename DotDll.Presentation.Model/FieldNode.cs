using DotDll.Model.Data.Members;

namespace DotDll.Presentation.Model
{
    public class FieldNode : MemberNode<Field>
    {

        public FieldNode(Field field) : base(field)
        {
            Name = BuildName();
        }

        private string BuildName()
        {
            var constraint = Member.FieldConstraint switch
            {
                Field.Constraint.None => "",
                Field.Constraint.ReadOnly => "readonly",
                Field.Constraint.Const => "const",
                _ => throw new System.ArgumentOutOfRangeException()
            };

            var declaration = GetAttributesString(Member);

            declaration += constraint != ""
                ? $"{GetAccessString(Member.AccessLevel)} {constraint} "
                : $"{GetAccessString(Member.AccessLevel)} ";


            declaration += $"{Member.ReturnType.FullName()} {Member.Name}";

            return declaration;
        }

        public override string Name { get; }
    }
}