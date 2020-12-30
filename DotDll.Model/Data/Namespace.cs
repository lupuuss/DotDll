using System.Collections.Generic;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedMember.Local

namespace DotDll.Model.Data
{
    public class Namespace
    {
        public Namespace(string name, List<Type> types)
        {
            Name = name;
            Types = types;
        }

        public Namespace(string name) : this(name, new List<Type>())
        {
        }

        private Namespace()
        {
            Name = null!;
            Types = null!;
        }
        
        public string Name { get; private set; }

        public List<Type> Types { get; private set; }

        public void AddType(Type type)
        {
            Types.Add(type);
        }
    }
}