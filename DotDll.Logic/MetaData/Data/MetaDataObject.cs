using System.Collections.Generic;

namespace DotDll.Logic.MetaData.Data
{
    public class MetaDataObject
    {
        public List<Namespace> Namespaces;

        internal MetaDataObject(string name, List<Namespace> namespaces)
        {
            Name = name;
            Namespaces = namespaces;
        }

        public string Name { get; internal set; }
    }
}