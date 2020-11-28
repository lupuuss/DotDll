using System.Collections.Generic;
using DotDll.Logic.MetaData.Data.Base;

namespace DotDll.Logic.MetaData.Data
{
    public class Namespace : Defined
    {
        public List<Type> Types { get; }

        internal Namespace(string definition, List<Type> types) : base(definition)
        {
            Types = types;
        }
    }
}