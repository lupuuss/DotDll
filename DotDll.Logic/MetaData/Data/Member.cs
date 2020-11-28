using System.Collections.Generic;
using DotDll.Logic.MetaData.Data.Base;

namespace DotDll.Logic.MetaData.Data
{
    public class Member : Defined
    {
        public List<Type> RelatedTypes { get; }

        internal Member(string definition,  List<Type> relatedTypes) : base(definition)
        {
            RelatedTypes = relatedTypes;
        }
    }
}