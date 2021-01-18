using System.Linq;
using DotDll.Model.Data.Members;

namespace DotDll.Presentation.Model
{
    public class MethodNode : MemberNode<Method>
    {

        public MethodNode(Method method) : base(method)
        {
            Name = BuildName();
        }

        private string BuildName()
        {
            var declaration = $"{GetAttributesString(Member)}{GetAccessString(Member.AccessLevel)} ";

            if (Member.IsStatic)
                declaration += "static ";
            else if (Member.IsAbstract)
                declaration += "abstract ";
            else if (Member.IsVirtual)
                declaration += "virtual ";

            if (Member.IsSealed) declaration += "sealed ";

            declaration += $"{Member.ReturnType.FullName()} {Member.Name}";

            if (Member.GenericArguments.Any())
                declaration += MapGenericArguments(Member.GenericArguments);

            declaration += MapParameters(Member.Parameters);

            return declaration;
        }

        public override string Name { get; }
    }
}