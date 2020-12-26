using System.Collections.Generic;

namespace DotDll.Model.Analysis.Data
{
    public class DllInfo
    {
        public string Name { get; }
        
        public List<Namespace> Namespaces { get; }

        public DllInfo(string name, List<Namespace> namespaces)
        {
            Name = name;
            Namespaces = namespaces;
        }

        public DllInfo(string name) : this(name, new List<Namespace>())
        {
        }

        internal void AddNamespace(Namespace nSpace)
        {
            Namespaces.Add(nSpace);
        }
    }
}