using System.Collections.Generic;
using DotDll.Model.Data.Base;

// ReSharper disable UnusedMember.Local

namespace DotDll.Model.Data.Members
{
    public class Constructor : Method
    {
        public Constructor(
            Type parentType, Access accessLevel, List<Parameter> parameters
        ) : base(".ctor", accessLevel, parentType, false, false)
        {
            Parameters = parameters;
        }

        private Constructor()
        {
        }
    }
}