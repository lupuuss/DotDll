using System.Collections.Generic;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedMember.Local

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

        public string Name { get; private set; }

        public List<Namespace> Namespaces { get; private set; }

        public void AddNamespace(Namespace nSpace)
        {
            Namespaces.Add(nSpace);
        }
    }
}