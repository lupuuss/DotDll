using System;
using System.Collections.Generic;
using System.Linq;
using DotDll.Model.Data.Base;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedMember.Local

namespace DotDll.Model.Data.Members
{
    public class Event : Member
    {
        public Event(
            string name, Method? removeMethod, Method? addMethod, Method? raiseMethod
        ) : base(name, Access.Inner, false, false)
        {
            if (removeMethod == null && addMethod == null)
                throw new ArgumentException("Add or remove method must not be null!");

            RemoveMethod = removeMethod;
            AddMethod = addMethod;
            RaiseMethod = raiseMethod;

            Method anyMethod = (addMethod ?? removeMethod)!;

            IsStatic = anyMethod.IsStatic;
            IsAbstract = anyMethod.IsAbstract;

            EventType = anyMethod.Parameters.First().ParameterType;
        }

        private Event()
        {
            EventType = null!;
        }

        public Method? RaiseMethod { get; private set; }
        
        public Method? AddMethod { get; private set; }
        
        public Method? RemoveMethod { get; private set; }
        
        public Type EventType { get; private set; }

        public override IEnumerable<Type> GetRelatedTypes()
        {
            return new List<Type>
            {
                EventType
            };
        }
    }
}