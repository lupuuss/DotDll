// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedMember.Local

using System.Collections.Generic;

namespace DotDll.Model.Data.Base
{
    public class Parameter
    {
        public Parameter(string name, Type parameterType)
        {
            Name = name;
            ParameterType = parameterType;
        }

        private Parameter()
        {
            Name = null!;
            ParameterType = null!;
        }

        public string Name { get; private set; }

        public Type ParameterType { get; private set; }

        public List<Attribute> Attributes { get; private set; } = new List<Attribute>();
    }
}