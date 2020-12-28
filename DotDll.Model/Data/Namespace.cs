using System.Collections.Generic;

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

        public string Name { get; }

        public List<Type> Types { get; }

        public void AddType(Type type)
        {
            Types.Add(type);
        }
    }
}