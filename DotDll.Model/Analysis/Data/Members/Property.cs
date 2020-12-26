﻿using System.Collections.Generic;
using DotDll.Model.Analysis.Data.Base;

namespace DotDll.Model.Analysis.Data.Members
{
    public class Property : Member
    {
        public Method Getter { get; }
        
        public Method Setter { get; }
       
        public bool CanRead { get; }
        
        public bool CanWrite { get; }

        internal Property(
            string name, bool isAbstract, Method getter, Method setter = null
            ) : base(name, Access.Inner, Kind.Property, getter.IsStatic, isAbstract)
        {
            Getter = getter;
            Setter = setter;
            CanRead = Getter != null;
            CanWrite = Setter != null;
        }

        public override List<Type> GetRelatedTypes()
        {

            return new List<Type>()
            {
                Getter.ReturnType
            };
        }
    }
}