using System.Collections.Generic;
using System.Runtime.Serialization;
using DotDll.Model.Serialization.File.Data.Base;

namespace DotDll.Model.Serialization.File.Data.Members
{
    [KnownType(typeof(SConstructor))]
    [DataContract(Namespace = "", Name = "ME")]
    public class SMethod : SMember
    {
        [DataMember(Name = "ga")] public List<SType> GenericArguments = null!;

        [DataMember(Name = "ise")] public bool IsSealed;

        [DataMember(Name = "iv")] public bool IsVirtual;

        [DataMember(Name = "p")] public List<SParameter> Parameters = null!;

        [DataMember(Name = "rt")] public SType ReturnType = null!;
    }
}