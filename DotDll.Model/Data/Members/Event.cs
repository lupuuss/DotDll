using System.Collections.Generic;
using System.Linq;
using DotDll.Model.Data.Base;

namespace DotDll.Model.Data.Members
{
    public class Event : Member
    {
        public Event(
            string name, bool isAbstract, Method removeMethod, Method addMethod, Method raiseMethod
        ) : base(name, Access.Inner, Kind.Event, addMethod.IsStatic, isAbstract)
        {
            RemoveMethod = removeMethod;
            AddMethod = addMethod;
            RaiseMethod = raiseMethod;
        }

        public Method RaiseMethod { get; }
        public Method AddMethod { get; }
        public Method RemoveMethod { get; }


        public override List<Type> GetRelatedTypes()
        {
            return new List<Type>
            {
                AddMethod.Parameters.First().ParameterType
            };
        }
    }
}