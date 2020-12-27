using System.Collections.Generic;
using DotDll.Model.Data.Base;

namespace DotDll.Model.Data.Members
{
    public class Field : Member
    {
        public Field(
            string name, Access accessLevel, Type returnType, bool isStatic
        ) : base(name, accessLevel, Kind.Field, isStatic, false)
        {
            ReturnType = returnType;
        }

        public Type ReturnType { get; }

        public override IEnumerable<Type> GetRelatedTypes()
        {
            return new List<Type>
            {
                ReturnType
            };
        }
    }
}