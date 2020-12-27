using System.Collections.Generic;
using DotDll.Model.Data;

namespace DotDll.Model.Serialization
{
    public interface IMetadataSerializer
    {
        List<string> GetAllIds();

        MetadataInfo Deserialize(string id);

        void Serialize(MetadataInfo metadataInfo);
    }
}