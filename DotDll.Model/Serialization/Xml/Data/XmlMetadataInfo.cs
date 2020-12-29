using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotDll.Model.Serialization.Xml.Data
{
    [DataContract(Namespace = "", IsReference = true)]
    public class XmlMetadataInfo
    {
        [DataMember]
        public string Name = null!;
        
        [DataMember]
        public List<XmlNamespace> Namespaces = null!;
    }
}