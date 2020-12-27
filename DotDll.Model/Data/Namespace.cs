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

        public string Name { get; }
        public List<Type> Types { get; }
    }
}