using System.Collections.Generic;
using DotDll.Model.Analysis.Data.Base;

namespace DotDll.Model.Analysis.Data.Members
{
    public class Field : Member
    {
        
        public Type ReturnType { get; }
        
        public Field(
            string name, Access accessLevel, Type returnType, bool isStatic
            ) : base(name, accessLevel, Kind.Field, isStatic, false)
        {
            ReturnType = returnType;
        }

        public override List<Type> GetRelatedTypes()
        {
            return new List<Type>()
            {
                ReturnType
            };
        }
    }
}