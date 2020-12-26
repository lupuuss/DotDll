using System.Collections.Generic;
using DotDll.Logic.MetaData.Data.Base;

namespace DotDll.Logic.MetaData.Data
{
    public class Type : Defined
    {
        internal Type(
            string definition,
            List<Member> members
        ) : base(definition)
        {
            Members = members;
        }

        internal Type(string definition) : this(definition, new List<Member>())
        {
        }

        public List<Member> Members { get; }
    }
}