using System.Collections.Generic;

namespace DotDll.Model.Data
{
    public class MetadataInfo
    {
        public MetadataInfo(string name, List<Namespace> namespaces)
        {
            Name = name;
            Namespaces = namespaces;
        }

        public MetadataInfo(string name) : this(name, new List<Namespace>())
        {
        }

        private MetadataInfo()
        {
            Name = null!;
            Namespaces = null!;
        }

        public string Name { get; }

        public List<Namespace> Namespaces { get; }

        internal void AddNamespace(Namespace nSpace)
        {
            Namespaces.Add(nSpace);
        }
    }
}