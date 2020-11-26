using System.Collections.Generic;
using DotDll.Logic.MetaData.Sources;

namespace DotDll.Logic.MetaData
{
    public interface IMetaDataService
    {
        bool IsValidSource(Source source);

        Source CreateFileSource(string path);

        List<Source> GetSerializedSources();

        MetaData LoadMetaData(Source source);
    }
}