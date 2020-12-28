using System.Collections.Generic;
using System.Linq;
using DotDll.Model.Data;

namespace DotDll.Model.Serialization
{
    public class XmlMetadataSerializer : IMetadataSerializer
    {
        public IEnumerable<string> GetAllIds()
        {
            return Enumerable.Empty<string>();
        }

        public MetadataInfo Deserialize(string id)
        {
            return null!;
        }

        public void Serialize(MetadataInfo metadataInfo)
        {
        }
    }
}