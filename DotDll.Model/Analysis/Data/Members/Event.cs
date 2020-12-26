using System;
using System.Collections.Generic;
using System.Linq;
using DotDll.Model.Analysis.Data.Base;

namespace DotDll.Model.Analysis.Data.Members
{
    public class Event : Member
    {
        public Method RaiseMethod { get; }
        public Method AddMethod { get; }
        public Method RemoveMethod { get; }

        public static event Action x;

        public Event(
            string name, bool isAbstract, Method removeMethod, Method addMethod, Method raiseMethod
            ) : base(name, Access.Inner, Kind.Event, addMethod.IsStatic, isAbstract)
        {
            RemoveMethod = removeMethod;
            AddMethod = addMethod;
            RaiseMethod = raiseMethod;
        }
        
        
        public override List<Type> GetRelatedTypes()
        {
            return new List<Type>()
            {
                AddMethod.Parameters.First().ParameterType
            };
        }
    }
}