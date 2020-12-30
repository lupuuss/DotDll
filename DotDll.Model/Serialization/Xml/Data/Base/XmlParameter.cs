using System.Runtime.Serialization;

namespace DotDll.Model.Serialization.Xml.Data.Base
{
 
    [DataContract(Namespace = "")]
    public class XmlParameter
    {
        [DataMember]
        public string Name = null!;

        [DataMember]
        public XmlType ParameterType = null!;
    }
}