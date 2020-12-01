using System.Collections.Generic;

namespace DotDll.Logic.MetaData.Data
{
    public class MetaDataObject
    {
        public readonly List<Namespace> Namespaces;

        public string Name { get; }
        
        internal MetaDataObject(string name, List<Namespace> namespaces)
        {
            Name = name;
            Namespaces = namespaces;
        }
    }
}