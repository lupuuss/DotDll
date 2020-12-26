using System.Collections.Generic;
using DotDll.Model.Analysis.Data.Base;

namespace DotDll.Model.Analysis.Data.Members
{

    public class NestedType : Member
    {
        
        public Type Type { get; }
        
        NestedType(Type type) : base(type.Name, type.Access, Kind.NestedType, type.IsStatic, type.IsAbstract)
        {
            Type = type;
        }

        public override List<Type> GetRelatedTypes()
        {
            return new List<Type>()
            {
                Type
            };
        }
    }
}