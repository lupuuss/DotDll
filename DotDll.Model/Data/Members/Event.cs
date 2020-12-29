using System;
using System.Collections.Generic;
using System.Linq;
using DotDll.Model.Data.Base;

namespace DotDll.Model.Data.Members
{
    public class Event : Member
    {
        public Event(
            string name, Method? removeMethod, Method? addMethod, Method? raiseMethod
        ) : base(name, Access.Inner, false, false)
        {
            if (removeMethod == null && addMethod == null && raiseMethod == null)
                throw new ArgumentException("One of event methods must not be null!");

            RemoveMethod = removeMethod;
            AddMethod = addMethod;
            RaiseMethod = raiseMethod;

            Method anyMethod = (addMethod ?? removeMethod ?? raiseMethod)!;

            IsStatic = anyMethod.IsStatic;
            IsAbstract = anyMethod.IsAbstract;

            EventType = anyMethod.Parameters.First().ParameterType;
        }

        private Event()
        {
            EventType = null!;
        }
        
        public Method? RaiseMethod { get; }
        public Method? AddMethod { get; }
        public Method? RemoveMethod { get; }
        public Type EventType { get; }

        public override IEnumerable<Type> GetRelatedTypes()
        {
            return new List<Type>
            {
                EventType
            };
        }
    }
}