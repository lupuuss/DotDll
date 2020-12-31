using System.Collections.Generic;
using System.Runtime.Serialization;
using DotDll.Model.Serialization.File.Data.Base;

namespace DotDll.Model.Serialization.File.Data.Members
{
    [KnownType(typeof(SConstructor))]
    [DataContract(Namespace = "", Name = "ME")]
    public class SMethod : SMember
    {
        [DataMember(Name = "iv")]
        public bool IsVirtual;

        [DataMember(Name = "ise")]
        public bool IsSealed;

        [DataMember(Name = "rt")]
        public SType ReturnType = null!;

        [DataMember(Name = "p")]
        public List<SParameter> Parameters = null!;

        [DataMember(Name = "ga")]
        public List<SType> GenericArguments = null!;
    }
}