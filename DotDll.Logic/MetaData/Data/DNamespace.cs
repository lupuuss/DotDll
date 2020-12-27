using System.Collections.Generic;
using DotDll.Logic.MetaData.Data.Base;

namespace DotDll.Logic.MetaData.Data
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