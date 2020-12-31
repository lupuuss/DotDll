using System.Runtime.Serialization;
using DotDll.Model.Serialization.Xml.Data.Base;

namespace DotDll.Model.Serialization.Xml.Data.Members
{
    [DataContract(Namespace = "", Name = "P")]
    public class XmlProperty : XmlMember
    {
        [DataMember(Name = "g")]
        public XmlMethod? Getter;

        [DataMember(Name = "s")]
        public XmlMethod? Setter;

        [DataMember(Name = "cr")]
        public bool CanRead;

        [DataMember(Name = "cw")]
        public bool CanWrite;
    }
}