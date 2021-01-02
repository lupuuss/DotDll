// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

using System.Collections.Generic;

namespace DotDll.Model.Data.Base
{
    public class Attribute
    {
        public Attribute(string name, Dictionary<string, string> values)
        {
            Name = name;
            Values = values;
        }

        private Attribute()
        {
            Name = null!;
            Values = null!;
        }

        public string Name { get; private set; }

        public Dictionary<string, string> Values { get; private set; }
    }
}