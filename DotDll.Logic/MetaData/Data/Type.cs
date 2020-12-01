﻿using System.Collections.Generic;
using DotDll.Logic.MetaData.Data.Base;

namespace DotDll.Logic.MetaData.Data
{
    public class Type : Defined
    {
        private Type(
            string definition,
            List<Member> members,
            bool isExternal
        ) : base(definition)
        {
            Members = members;
            IsExternal = isExternal;
        }

        public List<Member> Members { get; }
        public bool IsExternal { get; }

        internal static Type newInternalType(string definition, List<Member> members)
        {
            return new Type(definition, members, false);
        }

        internal static Type newExternalType(string definition)
        {
            return new Type(definition, new List<Member>(), false);
        }
    }
}