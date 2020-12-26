﻿using System.Collections.Generic;
using DotDll.Model.Analysis.Data.Base;

namespace DotDll.Model.Analysis.Data.Members
{
    public class Constructor : Method
    {
        public Constructor(
            Type parentType, Access accessLevel, List<Parameter> parameters
            ) : base(".ctor", accessLevel, parentType, Kind.Constructor, false, false, parameters, new List<Type>())
        {
        }
    }
}