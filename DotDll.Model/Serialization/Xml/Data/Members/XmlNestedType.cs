using System.Runtime.Serialization;
using DotDll.Model.Serialization.Xml.Data.Base;

namespace DotDll.Model.Serialization.Xml.Data.Members
{
    [DataContract(Namespace = "", Name = "NT")]
    public class XmlNestedType : XmlMember
    {
        [DataMember(Name = "t")]
        public XmlType Type = null!;
    }
}