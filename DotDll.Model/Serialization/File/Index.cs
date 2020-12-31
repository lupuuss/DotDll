using System;
using System.Collections.Generic;
using System.Linq;
using DotDll.Model.Files;

namespace DotDll.Model.Serialization.File
{
    public class Index
    {
        public SortedSet<string> SerializedFiles { get; } = new SortedSet<string>();

        public void Invalidate(IFilesManager filesManager, string filesPath, string ext)
        {
            SerializedFiles.RemoveWhere(
                fileName => !filesManager.FileExists(filesManager.FileInPath(filesPath, $"{fileName}.{ext}"))
            );
        }

        public string NextId(string metadataInfoName)
        {
            var lastTaken = SerializedFiles.LastOrDefault(name => name.Contains($"{metadataInfoName}_"));

            if (lastTaken == null) return $"{metadataInfoName}_0";

            lastTaken = lastTaken.Replace($"{metadataInfoName}_", "");

            return int.TryParse(lastTaken, out var result) 
                ? $"{metadataInfoName}_{result + 1}" 
                : $"{metadataInfoName}_{new Random().Next()}";
        }
    }
}