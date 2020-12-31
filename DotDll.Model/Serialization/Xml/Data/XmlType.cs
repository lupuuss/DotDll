
using System.Collections.Generic;
using System.Runtime.Serialization;
using DotDll.Model.Serialization.Xml.Data.Base;

namespace DotDll.Model.Serialization.Xml.Data
{
    [DataContract(Namespace = "", IsReference = true)]
    public class XmlType
    {
        [DataMember]
        public string Name = null!;

        [DataMember]
        public int Access;
        
        [DataMember]
        public int TypeKind;

        [DataMember]
        public bool IsSealed;

        [DataMember]
        public bool IsAbstract;

        [DataMember]
        public bool IsStatic;

        [DataMember]
        public List<XmlMember> Members = null!;
        
        [DataMember]
        public List<XmlType> GenericArguments = null!;

        [DataMember]
        public List<XmlType> GenericConstraints = null!;
    }
}