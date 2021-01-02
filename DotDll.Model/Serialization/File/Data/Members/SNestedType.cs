using System.Runtime.Serialization;
using DotDll.Model.Serialization.File.Data.Base;

namespace DotDll.Model.Serialization.File.Data.Members
{
    [DataContract(Namespace = "", Name = "NT")]
    public class SNestedType : SMember
    {
        [DataMember(Name = "t")] public SType Type = null!;
    }
}