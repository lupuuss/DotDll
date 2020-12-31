using System;
using System.Collections.Generic;
using DotDll.Model.Data.Base;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedMember.Local

namespace DotDll.Model.Data.Members
{
    public class Property : Member
    {
        public Property(
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

        public Method? Getter { get; private set; }

        public Method? Setter { get; private set; }

        public bool CanRead { get; private set; }

        public bool CanWrite { get; private set; }

        public Type ReturnType =>
            Getter?.ReturnType
            ?? Setter?.Parameters[0].ParameterType
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