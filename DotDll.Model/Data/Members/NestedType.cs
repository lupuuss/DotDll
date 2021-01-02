using System.Collections.Generic;
using DotDll.Model.Data.Base;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedMember.Local

namespace DotDll.Model.Data.Members
{
    public class NestedType : Member
    {
        public NestedType(Type type) : base(type.Name, type.Access, type.IsStatic, type.IsAbstract)
        {
            Type = type;
        }

        private NestedType()
        {
            Type = null!;
        }

        public Type Type { get; private set; }

        public override IEnumerable<Type> GetRelatedTypes()
        {
            return new List<Type>
            {
                Type
            };
        }
    }
}