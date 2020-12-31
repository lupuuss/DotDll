using System.Runtime.Serialization;
using DotDll.Model.Serialization.File.Xml.Data.Base;

namespace DotDll.Model.Serialization.File.Xml.Data.Members
{
    [DataContract(Namespace = "", Name = "E")]
    public class XmlEvent : XmlMember
    {
        [DataMember(Name = "ram")]
        public XmlMethod? RaiseMethod;
        
        [DataMember(Name = "am")]
        public XmlMethod? AddMethod;
        
        [DataMember(Name = "rem")]
        public XmlMethod? RemoveMethod;
        
        [DataMember(Name = "et")]
        public XmlType EventType = null!;
    }
}