using System.Collections.Generic;
using DotDll.Logic.MetaData.Data.Base;

namespace DotDll.Logic.MetaData.Data
{
    public class DType : Declared
    {
        internal DType(
            string declaration,
            List<DMember> members
        ) : base(declaration)
        {
            Members = members;
        }

        internal DType(string declaration) : this(declaration, new List<DMember>())
        {
        }

        public List<DMember> Members { get; }
    }
}