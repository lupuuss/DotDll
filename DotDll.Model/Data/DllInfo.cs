using System.Collections.Generic;

namespace DotDll.Model.Data
{
    public class DllInfo
    {
        public DllInfo(string name, List<Namespace> namespaces)
        {
            Name = name;
            Namespaces = namespaces;
        }

        public DllInfo(string name) : this(name, new List<Namespace>())
        {
        }

        public string Name { get; }

        public List<Namespace> Namespaces { get; }

        internal void AddNamespace(Namespace nSpace)
        {
            Namespaces.Add(nSpace);
        }
    }
}