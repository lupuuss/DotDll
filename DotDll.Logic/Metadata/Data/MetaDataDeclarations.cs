using System.Collections.Generic;

namespace DotDll.Logic.Metadata.Data
{
    public class MetaDataDeclarations
    {
        public readonly List<DNamespace> Namespaces;

        internal MetaDataDeclarations(string name, List<DNamespace> namespaces)
        {
            Name = name;
            Namespaces = namespaces;
        }

        public string Name { get; }
    }
}