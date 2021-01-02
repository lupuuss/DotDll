using System.Runtime.Serialization;
using DotDll.Model.Serialization.File.Data.Base;

namespace DotDll.Model.Serialization.File.Data.Members
{
    [DataContract(Namespace = "", Name = "E")]
    public class SEvent : SMember
    {
        [DataMember(Name = "am")] public SMethod? AddMethod;

        [DataMember(Name = "et")] public SType EventType = null!;

        [DataMember(Name = "ram")] public SMethod? RaiseMethod;

        [DataMember(Name = "rem")] public SMethod? RemoveMethod;
    }
}