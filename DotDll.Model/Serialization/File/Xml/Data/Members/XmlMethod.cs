using System.Collections.Generic;
using System.Runtime.Serialization;
using DotDll.Model.Serialization.File.Xml.Data.Base;

namespace DotDll.Model.Serialization.File.Xml.Data.Members
{
    [KnownType(typeof(XmlConstructor))]
    [DataContract(Namespace = "", Name = "ME")]
    public class XmlMethod : XmlMember
    {
        [DataMember(Name = "iv")]
        public bool IsVirtual;

        [DataMember(Name = "ise")]
        public bool IsSealed;

        [DataMember(Name = "rt")]
        public XmlType ReturnType = null!;

        [DataMember(Name = "p")]
        public List<XmlParameter> Parameters = null!;

        [DataMember(Name = "ga")]
        public List<XmlType> GenericArguments = null!;
    }
}