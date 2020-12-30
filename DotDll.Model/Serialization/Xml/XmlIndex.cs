using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DotDll.Model.Files;

namespace DotDll.Model.Serialization.Xml
{
    [DataContract]
    public class XmlIndex
    {
    
        [DataMember]
        public SortedSet<string> SerializedFiles { get; set; } = new SortedSet<string>();

        public void Invalidate(IFilesManager filesManager, string filesPath)
        {
            SerializedFiles.RemoveWhere(
                fileName => !filesManager.FileExists(filesManager.FileInPath(filesPath, fileName))
                );
        }

        public string NextFileName(string metadataInfoName)
        {
            var lastTaken = SerializedFiles.LastOrDefault(name => name.Contains($"{metadataInfoName}_"));

            if (lastTaken == null) return $"{metadataInfoName}_0.xml";

            lastTaken = lastTaken.Replace($"{metadataInfoName}_", "");
            lastTaken = lastTaken.Replace(".xml", "");

            return int.TryParse(lastTaken, out var result) 
                ? $"{metadataInfoName}_{result + 1}.xml" 
                : $"{metadataInfoName}_{new Random().Next()}";
        }
    }
}