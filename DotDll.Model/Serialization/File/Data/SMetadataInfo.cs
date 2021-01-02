using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotDll.Model.Serialization.File.Data
{
    [DataContract(Namespace = "")]
    public class SMetadataInfo
    {
        [DataMember(Name = "n")] public string Name = null!;

        [DataMember(Name = "ns")] public List<SNamespace> Namespaces = null!;
    }
}