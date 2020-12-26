using System.Collections.Generic;
using System.Threading.Tasks;
using DotDll.Logic.MetaData.Data;
using DotDll.Logic.MetaData.Sources;

namespace DotDll.Logic.MetaData
{
    public interface IMetaDataService
    {
        bool IsValidFileSourcePath(string path);

        Source CreateFileSource(string path);

        Task<List<Source>> GetSerializedSources();

        Task<MetaDataObject> LoadMetaData(Source source);

        Task<bool> SaveMetaData(Source source);
    }
}