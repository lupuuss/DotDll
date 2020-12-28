using System.Collections.Generic;

namespace DotDll.Model.Data.Base
{
    public abstract class Member
    {
        internal Member(string name, Access accessLevel, bool isStatic, bool isAbstract)
        {
            Name = name;
            AccessLevel = accessLevel;
            IsStatic = isStatic;
            IsAbstract = isAbstract;
        }

        public string Name { get; }

        public Access AccessLevel { get; }

        public bool IsStatic { get; protected set; }

        public bool IsAbstract { get; protected set; }

        public abstract IEnumerable<Type> GetRelatedTypes();
    }
}