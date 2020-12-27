using DotDll.Logic.Metadata.Data;
using DotDll.Model.Data;

namespace DotDll.Logic.Metadata.Map
{
    public interface IMetadataMapper
    {
        MetaDataDeclarations Map(DllInfo dllInfo);
    }
}