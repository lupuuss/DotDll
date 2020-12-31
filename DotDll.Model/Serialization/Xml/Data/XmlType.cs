
using System.Collections.Generic;
using System.Runtime.Serialization;
using DotDll.Model.Serialization.Xml.Data.Base;

namespace DotDll.Model.Serialization.Xml.Data
{
    [DataContract(Namespace = "", IsReference = true, Name = "T")]
    public class XmlType
    {
        [DataMember(Name = "n")]
        public string Name = null!;

        [DataMember(Name = "a")]
        public int Access;
        
        [DataMember(Name = "tk")]
        public int TypeKind;

        [DataMember(Name = "is")]
        public bool IsSealed;

        [DataMember(Name = "ia")]
        public bool IsAbstract;

        [DataMember(Name = "ist")]
        public bool IsStatic;

        [DataMember(Name = "m")]
        public List<XmlMember> Members = null!;
        
        [DataMember(Name = "ga")]
        public List<XmlType> GenericArguments = null!;

        [DataMember(Name = "gc")]
        public List<XmlType> GenericConstraints = null!;
        
        [DataMember(Name = "bt")]
        public List<XmlType> BaseTypes = null!;
    }
}