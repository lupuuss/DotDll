using System.Runtime.Serialization;
using DotDll.Model.Serialization.Xml.Data.Base;

namespace DotDll.Model.Serialization.Xml.Data.Members
{
    [DataContract(Namespace = "")]
    public class XmlProperty : XmlMember
    {
        [DataMember]
        public XmlMethod? Getter;

        [DataMember]
        public XmlMethod? Setter;

        [DataMember]
        public bool CanRead;

        [DataMember]
        public bool CanWrite;
    }
}