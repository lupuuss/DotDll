using System;
using System.Collections.Generic;
using DotDll.Model.Data.Base;

namespace DotDll.Model.Data.Members
{
    public class Property : Member
    {
        
        internal Property(
            string name, bool isAbstract, Method? getter = null, Method? setter = null
        ) : base(
            name,
            Access.Inner,
            getter?.IsStatic ?? setter?.IsStatic ?? false, 
            getter?.IsAbstract ?? setter?.IsAbstract ?? false
            )
        {
            if (getter == null && setter == null) throw new ArgumentException("Getter or setter must be not null!");
            
            Getter = getter;
            Setter = setter;
            CanRead = Getter != null;
            CanWrite = Setter != null;
        }

        public Method? Getter { get; }

        public Method? Setter { get; }

        public bool CanRead { get; }

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