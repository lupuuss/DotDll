using System.Runtime.Serialization;
using DotDll.Model.Serialization.File.Data.Base;

namespace DotDll.Model.Serialization.File.Data.Members
{
    [DataContract(Namespace = "", Name = "P")]
    public class SProperty : SMember
    {
        [DataMember(Name = "cr")] public bool CanRead;

        [DataMember(Name = "cw")] public bool CanWrite;

        [DataMember(Name = "g")] public SMethod? Getter;

        [DataMember(Name = "s")] public SMethod? Setter;
    }
}