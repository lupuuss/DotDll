using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotDll.Model.Serialization.File.Json.Data
{
    [DataContract(Namespace = "")]
    public class JsonMetadataInfo
    {
        [DataMember(Name = "n")]
        public string Name = null!;
        
        [DataMember(Name = "ns")]
        public List<JsonNamespace> Namespaces = null!;
    }
}