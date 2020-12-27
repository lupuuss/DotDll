using System.Collections.Generic;
using DotDll.Model.Data.Base;

namespace DotDll.Model.Data.Members
{
    public class Property : Member
    {
        internal Property(
            string name, bool isAbstract, Method getter, Method? setter = null
        ) : base(name, Access.Inner, Kind.Property, getter.IsStatic, isAbstract)
        {
            Getter = getter;
            Setter = setter;
            CanWrite = Setter != null;
        }

        public Method Getter { get; }

        public Method? Setter { get; }

        public bool CanRead { get; } = true;

        public bool CanWrite { get; }

        public override IEnumerable<Type> GetRelatedTypes()
        {
            return new List<Type>
            {
                Getter.ReturnType
            };
        }
    }
}