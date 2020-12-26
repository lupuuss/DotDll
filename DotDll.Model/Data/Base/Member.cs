using System.Collections.Generic;

namespace DotDll.Model.Data.Base
{
    public abstract class Member
    {

        public enum Kind
        {
            Method, Property, NestedType, Field, Constructor, Event
        }

        public string Name { get; }
        
        public Access AccessLevel { get; }

        public Kind MemberKind { get; }
        
        public bool IsStatic { get; }
        
        public bool IsAbstract { get; }
        
        internal Member(string name, Access accessLevel, Kind memberKind, bool isStatic, bool isAbstract)
        {
            Name = name;
            AccessLevel = accessLevel;
            MemberKind = memberKind;
            IsStatic = isStatic;
            IsAbstract = isAbstract;
        }

        public abstract List<Type> GetRelatedTypes();
    }
}