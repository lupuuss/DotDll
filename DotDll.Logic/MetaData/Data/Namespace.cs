using System.Collections.Generic;
using DotDll.Logic.MetaData.Data.Base;

namespace DotDll.Logic.MetaData.Data
{
    public class Namespace : Defined
    {
        internal Namespace(string definition, List<Type> types) : base(definition)
        {
            Types = types;
        }

        public List<Type> Types { get; }
    }
}