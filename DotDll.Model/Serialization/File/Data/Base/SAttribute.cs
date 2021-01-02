using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotDll.Model.Serialization.File.Data.Base
{
    [DataContract(Namespace = "", Name = "Atr")]
    public class SAttribute
    {
        [DataMember(Name = "n")] public string Name = null!;

        [DataMember(Name = "v")] public Dictionary<string, string> Values = null!;
    }
}