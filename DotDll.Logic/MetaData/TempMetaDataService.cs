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
                Thread.Sleep(1000);
                return new List<Source>
                {
                    new SerializedSource("Example1"),
                    new SerializedSource("Example2"),
                    new SerializedSource("Example3"),
                    new SerializedSource("Example4"),
                    new SerializedSource("Example5"),
                    new SerializedSource("Example6"),
                    new SerializedSource("Example7"),
                    new SerializedSource("Example8"),
                    new SerializedSource("Example9"),
                };
            });
        }

        public Task<MetaData> LoadMetaData(Source source)
        {
            return Task.Run(() =>
            {
                Thread.Sleep(1000);
                return new MetaData
                {
                    Name = source.Identifier
                };
            });
        }
    }
}