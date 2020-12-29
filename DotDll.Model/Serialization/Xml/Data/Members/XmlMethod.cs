using System.Collections.Generic;
using System.Runtime.Serialization;
using DotDll.Model.Serialization.Xml.Data.Base;

namespace DotDll.Model.Serialization.Xml.Data.Members
{
    [KnownType(typeof(XmlConstructor))]
    [DataContract(Namespace = "", IsReference = true)]
    public class XmlMethod : XmlMember
    {
        [DataMember]
        public bool IsVirtual;

        [DataMember]
        public bool IsSealed;

        [DataMember]
        public XmlType ReturnType = null!;

        [DataMember]
        public List<XmlParameter> Parameters = null!;

        [DataMember]
        public List<XmlType> GenericArguments = null!;
    }
}