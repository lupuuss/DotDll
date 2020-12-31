using System.Runtime.Serialization;

namespace DotDll.Model.Serialization.File.Xml.Data.Base
{
 
    [DataContract(Namespace = "", Name = "PA")]
    public class XmlParameter
    {
        [DataMember(Name = "n")]
        public string Name = null!;

        [DataMember(Name = "pt")]
        public XmlType ParameterType = null!;
    }
}