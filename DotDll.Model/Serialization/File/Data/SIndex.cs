using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotDll.Model.Serialization.File.Data
{
    [DataContract]
    public class SIndex
    {
        [DataMember] public SortedSet<string> SerializedFiles { get; set; } = new SortedSet<string>();
    }
}