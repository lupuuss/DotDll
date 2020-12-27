using System.Collections.Generic;
using DotDll.Logic.MetaData.Data.Base;

namespace DotDll.Logic.MetaData.Data
{
    public class DMember : Declared
    {
        internal DMember(string declaration, List<DType> relatedTypes) : base(declaration)
        {
            RelatedTypes = relatedTypes;
        }

        public List<DType> RelatedTypes { get; }
    }
}