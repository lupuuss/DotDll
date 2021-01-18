using System.Collections.Generic;
using System.Threading.Tasks;
using DotDll.Logic.Metadata.Sources;
using DotDll.Model.Data;

namespace DotDll.Logic.Metadata
{
    public interface IMetadataService
    {
        bool IsValidFileSourcePath(string path);

        Source CreateFileSource(string path);

        Task<List<Source>> GetSerializedSources();

        Task<MetadataInfo> LoadMetadata(Source source);

        Task<bool> SaveMetadata(Source source);
    }
}