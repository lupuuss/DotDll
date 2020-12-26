using System.Collections.Generic;

namespace DotDll.Model.Analysis.Data
{
    public class Namespace
    {
        public string Name { get; }
        public List<Type> Types { get; }

        public Namespace(string name, List<Type> types)
        {
            Name = name;
            Types = types;
        }
    }
}