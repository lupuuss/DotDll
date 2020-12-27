using System.Collections.Generic;
using System.Linq;
using DotDll.Model.Data.Base;

namespace DotDll.Model.Data.Members
{
    public class Event : Member
    {
        public Event(
            string name, bool isAbstract, Method removeMethod, Method addMethod, Method raiseMethod
        ) : base(name, Access.Inner, addMethod.IsStatic, isAbstract)
        {
            RemoveMethod = removeMethod;
            AddMethod = addMethod;
            RaiseMethod = raiseMethod;
        }

        public Method RaiseMethod { get; }
        public Method AddMethod { get; }
        public Method RemoveMethod { get; }

        public override IEnumerable<Type> GetRelatedTypes()
        {
            return new List<Type>
            {
                AddMethod.Parameters.First().ParameterType
            };
        }
    }
}