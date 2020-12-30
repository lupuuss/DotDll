using System.Runtime.Serialization;
using DotDll.Model.Serialization.Xml.Data.Base;

namespace DotDll.Model.Serialization.Xml.Data.Members
{
    [DataContract(Namespace = "")]
    public class XmlEvent : XmlMember
    {
        [DataMember]
        public XmlMethod? RaiseMethod;
        
        [DataMember]
        public XmlMethod? AddMethod;
        
        [DataMember]
        public XmlMethod? RemoveMethod;
        
        [DataMember]
        public XmlType EventType = null!;
    }
}