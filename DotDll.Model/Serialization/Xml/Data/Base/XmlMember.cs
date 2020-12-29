using System.Runtime.Serialization;
using DotDll.Model.Serialization.Xml.Data.Members;

namespace DotDll.Model.Serialization.Xml.Data.Base
{

    [KnownType(typeof(XmlEvent))]
    [KnownType(typeof(XmlField))]
    [KnownType(typeof(XmlMethod))]
    [KnownType(typeof(XmlNestedType))]
    [KnownType(typeof(XmlProperty))]
    [DataContract(Namespace = "", IsReference = true)]
    public abstract class XmlMember
    {
        [DataMember]
        public string Name = null!;

        [DataMember]
        public int AccessLevel;

        [DataMember]
        public bool IsStatic;

        [DataMember]
        public bool IsAbstract;
    }
}