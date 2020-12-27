using System.Collections.Generic;
using DotDll.Logic.Metadata.Data.Base;

namespace DotDll.Logic.Metadata.Data
{
    public class DNamespace : Declared
    {
        internal DNamespace(string declaration, List<DType> types) : base(declaration)
        {
            Types = types;
        }

        public List<DType> Types { get; }
    }
}