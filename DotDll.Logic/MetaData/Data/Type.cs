using System.Collections.Generic;
using DotDll.Logic.MetaData.Data.Base;

namespace DotDll.Logic.MetaData.Data
{
    public class Type : Defined
    {
        private Type(
            string definition,
            List<Member> members
        ) : base(definition)
        {
            Members = members;
        }

        public List<Member> Members { get; }

        internal static Type NewInternalType(string definition, List<Member> members)
        {
            return new Type(definition, members);
        }

        internal static Type NewExternalType(string definition)
        {
            return new Type(definition, new List<Member>());
        }
    }
}