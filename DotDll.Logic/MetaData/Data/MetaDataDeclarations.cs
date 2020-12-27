using System.Collections.Generic;

namespace DotDll.Logic.MetaData.Data
{
    public class MetaDataDeclarations
    {
        public readonly List<DNamespace> Namespaces;

        public string Name { get; }
        
        internal MetaDataDeclarations(string name, List<DNamespace> namespaces)
        {
            Name = name;
            Namespaces = namespaces;
        }
    }
}