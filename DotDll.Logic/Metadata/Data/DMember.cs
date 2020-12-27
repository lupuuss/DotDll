using System.Collections.Generic;
using DotDll.Logic.Metadata.Data.Base;

namespace DotDll.Logic.Metadata.Data
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