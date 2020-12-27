using System.Collections.Generic;
using DotDll.Model.Data.Base;

namespace DotDll.Model.Data.Members
{
    public class Property : Member
    {
        internal Property(
            string name, bool isAbstract, Method getter, Method setter = null
        ) : base(name, Access.Inner, Kind.Property, getter.IsStatic, isAbstract)
        {
            Getter = getter;
            Setter = setter;
            CanRead = Getter != null;
            CanWrite = Setter != null;
        }

        public Method Getter { get; }

        public Method Setter { get; }

        public bool CanRead { get; }

        public bool CanWrite { get; }

        public override List<Type> GetRelatedTypes()
        {
            return new List<Type>
            {
                Getter.ReturnType
            };
        }
    }
}