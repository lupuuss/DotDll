using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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

        public Task<List<Source>> GetSerializedSources()
        {
            return Task.Run(delegate
            {
                Thread.Sleep(3000);
                return new List<Source>
                {
                    new SerializedSource("Example1"),
                    new SerializedSource("Example2"),
                    new SerializedSource("Example3"),
                    new SerializedSource("Example4")
                };
            });
        }

        public Task<MetaData> LoadMetaData(Source source)
        {
            Thread.Sleep(3000);
            return Task.Run(() => new MetaData
            {
                Name = source.Identifier
            });
        }
    }
}