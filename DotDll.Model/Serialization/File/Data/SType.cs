using System.Collections.Generic;
using System.Runtime.Serialization;
using DotDll.Model.Serialization.File.Data.Base;

namespace DotDll.Model.Serialization.File.Data
{
    [DataContract(Namespace = "", IsReference = true, Name = "T")]
    public class SType
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
        public List<SMember> Members = null!;
        
        [DataMember(Name = "ga")]
        public List<SType> GenericArguments = null!;

        [DataMember(Name = "gc")]
        public List<SType> GenericConstraints = null!;
        
        [DataMember(Name = "bt")]
        public List<SType> BaseTypes = null!;

        [DataMember(Name = "atr")] 
        public List<SAttribute> Attributes = null!;
    }
}