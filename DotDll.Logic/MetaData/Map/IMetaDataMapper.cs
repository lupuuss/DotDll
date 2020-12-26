using DotDll.Logic.MetaData.Data;
using DotDll.Model.Data;

namespace DotDll.Logic.MetaData.Map
{
    public interface IMetaDataMapper
    {
        MetaDataObject Map(DllInfo dllInfo);
    }
}