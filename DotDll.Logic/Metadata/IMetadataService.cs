using System.Collections.Generic;
using System.Threading.Tasks;
using DotDll.Logic.Metadata.Data;
using DotDll.Logic.Metadata.Sources;

namespace DotDll.Logic.Metadata
{
    public interface IMetadataService
    {
        bool IsValidFileSourcePath(string path);

        Source CreateFileSource(string path);

        Task<List<Source>> GetSerializedSources();

        Task<MetadataDeclarations> LoadMetadata(Source source);

        Task<bool> SaveMetadata(Source source);
    }
}