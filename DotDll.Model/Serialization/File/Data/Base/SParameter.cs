using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotDll.Model.Serialization.File.Data.Base
{
 
    [DataContract(Namespace = "", Name = "PA")]
    public class SParameter
    {
        [DataMember(Name = "n")]
        public string Name = null!;

        [DataMember(Name = "pt")]
        public SType ParameterType = null!;
        
        [DataMember(Name = "atr")] 
        public List<SAttribute> Attributes = null!;
        
    }
}