
using System.Runtime.Serialization;
using DotDll.Model.Serialization.Xml.Data.Base;

namespace DotDll.Model.Serialization.Xml.Data.Members 
{
    [DataContract(Namespace = "", Name = "F")]
    public class XmlField : XmlMember
    {
        [DataMember(Name = "fc")]
        public int FieldConstraint;

        [DataMember(Name = "rt")]
        public XmlType ReturnType = null!;
    }
}