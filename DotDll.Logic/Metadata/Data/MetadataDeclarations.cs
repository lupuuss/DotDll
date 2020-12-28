using System.Collections.Generic;

namespace DotDll.Logic.Metadata.Data
{
    public class MetadataDeclarations
    {
        public readonly List<DNamespace> Namespaces;

        public MetadataDeclarations(string name, List<DNamespace> namespaces)
        {
            Name = name;
            Namespaces = namespaces;
        }

        public string Name { get; }
    }
}