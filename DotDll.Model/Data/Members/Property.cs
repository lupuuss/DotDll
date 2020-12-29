using System;
using System.Collections.Generic;
using DotDll.Model.Data.Base;

namespace DotDll.Model.Data.Members
{
    public class Property : Member
    {
        internal Property(
            string name, Method? getter = null, Method? setter = null
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

        private Property()
        {
        }

        public Method? Getter { get; }

        public Method? Setter { get; }

        public bool CanRead { get; }

        public bool CanWrite { get; }

        public Type ReturnType =>
            Getter?.ReturnType
            ?? Setter?.ReturnType
            ?? throw new InvalidOperationException("Getter or setter must be not null!");

        public override IEnumerable<Type> GetRelatedTypes()
        {
            return new List<Type>
            {
                ReturnType
            };
        }
    }
}