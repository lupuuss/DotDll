using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotDll.Model.Serialization.File.Xml.Data
{
    [DataContract]
    public class XmlIndex
    {
    
        [DataMember]
        public SortedSet<string> SerializedFiles { get; set; } = new SortedSet<string>();
    }
}