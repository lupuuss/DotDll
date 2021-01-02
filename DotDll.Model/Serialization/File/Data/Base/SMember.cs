using System.Collections.Generic;
using System.Runtime.Serialization;
using DotDll.Model.Serialization.File.Data.Members;

namespace DotDll.Model.Serialization.File.Data.Base
{
    [KnownType(typeof(SEvent))]
    [KnownType(typeof(SField))]
    [KnownType(typeof(SMethod))]
    [KnownType(typeof(SNestedType))]
    [KnownType(typeof(SProperty))]
    [DataContract(Namespace = "", Name = "M")]
    public abstract class SMember
    {
        [DataMember(Name = "al")] public int AccessLevel;

        [DataMember(Name = "atr")] public List<SAttribute> Attributes = null!;

        [DataMember(Name = "ia")] public bool IsAbstract;

        [DataMember(Name = "is")] public bool IsStatic;

        [DataMember(Name = "n")] public string Name = null!;
    }
}