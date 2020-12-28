using System.Collections.Generic;
using System.Linq;
using DotDll.Model.Data.Base;

namespace DotDll.Model.Data
{
    public class Type
    {
        public enum Kind
        {
            Interface,
            Class,
            Enum,
            Array,
            GenericArg
        }

        private readonly string _name;

        internal Type(
            string name,
            Access access,
            Kind typeKind,
            bool isSealed,
            bool isAbstract,
            List<Member> members,
            List<Type> genericArguments
        )
        {
            _name = name;
            Access = access;
            TypeKind = typeKind;
            IsSealed = isSealed;
            IsAbstract = isAbstract;
            IsStatic = isSealed && isAbstract;
            Members = members;
            GenericArguments = genericArguments;
        }

        public string Name
        {
            get
            {
                if (GenericArguments.Any())
                    return _name + $"<{string.Join(", ", GenericArguments.Select(arg => arg.Name))}>";

                return _name;
            }
        }

        public Access Access { get; }

        public Kind TypeKind { get; }

        public bool IsSealed { get; }

        public bool IsAbstract { get; }

        public bool IsStatic { get; }

        public List<Member> Members { get; }

        public List<Type> GenericArguments { get; }

        internal void AddMember(Member member)
        {
            Members.Add(member);
        }
    }
}