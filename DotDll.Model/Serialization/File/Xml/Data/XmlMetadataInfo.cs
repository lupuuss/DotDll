using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotDll.Model.Serialization.File.Xml.Data
{
    [DataContract(Namespace = "")]
    public class XmlMetadataInfo
    {
        [DataMember(Name = "n")]
        public string Name = null!;
        
        [DataMember(Name = "ns")]
        public List<XmlNamespace> Namespaces = null!;
    }
}