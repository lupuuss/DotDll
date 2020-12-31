using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotDll.Model.Serialization.File.Data
{
    [DataContract(Namespace = "", Name = "N")]
    public class SNamespace
    {
        [DataMember(Name = "n")]
        public string Name = null!;

        [DataMember(Name = "t")]
        public List<SType> Types = null!;
    }
}