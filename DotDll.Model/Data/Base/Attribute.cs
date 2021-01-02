// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

using System.Collections.Generic;
using System.Linq;

namespace DotDll.Model.Data.Base
{
    public class Attribute
    {
        public string Name { get; private set; }

        public Dictionary<string, string> Values;
        
        public Attribute(string name, Dictionary<string, string> values)
        {
            Name = name;
            Values = values;
        }

        public string FullName()
        {
            return Name + $"({string.Join(", ",Values.Select(p => p.Key + " = " + p.Value))})";
        }
    }
}