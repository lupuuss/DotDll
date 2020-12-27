using System.Collections.Generic;
using DotDll.Model.Data.Base;

namespace DotDll.Model.Data.Members
{
    public class NestedType : Member
    {
        private NestedType(Type type) : base(type.Name, type.Access, Kind.NestedType, type.IsStatic, type.IsAbstract)
        {
            Type = type;
        }

        public Type Type { get; }

        public override List<Type> GetRelatedTypes()
        {
            return new List<Type>
            {
                Type
            };
        }
    }
}