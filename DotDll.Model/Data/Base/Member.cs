using System.Collections.Generic;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedMember.Local

namespace DotDll.Model.Data.Base
{
    public abstract class Member
    {
        protected Member(string name, Access accessLevel, bool isStatic, bool isAbstract)
        {
            Name = name;
            AccessLevel = accessLevel;
            IsStatic = isStatic;
            IsAbstract = isAbstract;
        }

        protected Member()
        {
            Name = null!;
        }

        public string Name { get; private set; }

        public Access AccessLevel { get; private set; }

        public bool IsStatic { get; protected set; }

        public bool IsAbstract { get; protected set; }

        public List<Attribute> Attributes { get; private set; } = new List<Attribute>();

        public abstract IEnumerable<Type> GetRelatedTypes();
    }
}