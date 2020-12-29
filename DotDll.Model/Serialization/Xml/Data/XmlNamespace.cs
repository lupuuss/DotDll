using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotDll.Model.Serialization.Xml.Data
{
    [DataContract(Namespace = "", IsReference = true)]
    public class XmlNamespace
    {
        [DataMember]
        public string Name = null!;

        [DataMember]
        public List<XmlType> Types = null!;
    }
}