
using System.Runtime.Serialization;
using DotDll.Model.Serialization.Xml.Data.Base;

namespace DotDll.Model.Serialization.Xml.Data.Members 
{
    [DataContract(Namespace = "", IsReference = true)]
    public class XmlField : XmlMember
    {
        [DataMember]
        public int FieldConstraint;

        [DataMember]
        public XmlType ReturnType = null!;
    }
}