using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotDll.Model.Serialization.Xml.Data
{
    [DataContract(Namespace = "", Name = "N")]
    public class XmlNamespace
    {
        [DataMember(Name = "n")]
        public string Name = null!;

        [DataMember(Name = "t")]
        public List<XmlType> Types = null!;
    }
}