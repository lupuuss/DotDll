using System.Collections.Generic;
using System.IO;
using DotDll.Logic.MetaData.Sources;

namespace DotDll.Logic.MetaData
{
    public class TempMetaDataService : IMetaDataService
    {
        public bool IsValidFileSourcePath(string path)
        {
            return File.Exists(path);
        }

        public Source CreateFileSource(string path)
        {
            return new FileSource(path);
        }

        public List<Source> GetSerializedSources()
        {
            return new List<Source>
            {
                new SerializedSource("Example1"),
                new SerializedSource("Example2"),
                new SerializedSource("Example3"),
                new SerializedSource("Example4")
            };
        }

        public MetaData LoadMetaData(Source source)
        {
            return new MetaData
            {
                Name = source.Identifier
            };
        }
    }
}