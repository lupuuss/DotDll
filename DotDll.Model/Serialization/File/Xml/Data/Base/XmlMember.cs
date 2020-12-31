using System.Runtime.Serialization;
using DotDll.Model.Serialization.File.Xml.Data.Members;

namespace DotDll.Model.Serialization.File.Xml.Data.Base
{

    [KnownType(typeof(XmlEvent))]
    [KnownType(typeof(XmlField))]
    [KnownType(typeof(XmlMethod))]
    [KnownType(typeof(XmlNestedType))]
    [KnownType(typeof(XmlProperty))]
    [DataContract(Namespace = "", Name = "M")]
    public abstract class XmlMember
    {
        [DataMember(Name = "n")]
        public string Name = null!;

        [DataMember(Name = "al")]
        public int AccessLevel;

        [DataMember(Name = "is")]
        public bool IsStatic;

        [DataMember(Name = "ia")]
        public bool IsAbstract;
    }
}