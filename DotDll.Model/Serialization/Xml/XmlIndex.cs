using System.Collections.Generic;
using System.Runtime.Serialization;
using DotDll.Model.Files;

namespace DotDll.Model.Serialization.Xml
{
    [DataContract]
    public class XmlIndex
    {
    
        [DataMember]
        public List<string> SerializedFiles { get; set; } = new List<string>();

        public void Invalidate(IFilesManager filesManager, string filesPath)
        {
            SerializedFiles.RemoveAll(
                fileName => !filesManager.FileExists(filesManager.FileInPath(filesPath, fileName))
                );
        }
    }
}