using System.Runtime.Serialization;
using DotDll.Model.Serialization.File.Data.Base;

namespace DotDll.Model.Serialization.File.Data.Members 
{
    [DataContract(Namespace = "", Name = "F")]
    public class SField : SMember
    {
        [DataMember(Name = "fc")]
        public int FieldConstraint;

        [DataMember(Name = "rt")]
        public SType ReturnType = null!;
    }
}