using DotDll.Logic.Metadata.Data;
using DotDll.Model.Data;

namespace DotDll.Logic.Metadata.Map
{
    public interface IMetadataMapper
    {
        MetadataDeclarations Map(MetadataInfo metadataInfo);
    }
}