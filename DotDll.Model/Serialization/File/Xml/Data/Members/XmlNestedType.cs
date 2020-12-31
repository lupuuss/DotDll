using System.Runtime.Serialization;
using DotDll.Model.Serialization.File.Xml.Data.Base;

namespace DotDll.Model.Serialization.File.Xml.Data.Members
{
    [DataContract(Namespace = "", Name = "NT")]
    public class XmlNestedType : XmlMember
    {
        [DataMember(Name = "t")]
        public XmlType Type = null!;
    }
}