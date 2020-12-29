using DotDll.Model.Data;
using DotDll.Model.Serialization.Xml.Data;

namespace DotDll.Model.Serialization.Xml.Map
{
    public interface IXmlMapper
    {
        XmlMetadataInfo Map(MetadataInfo metadataInfo);

        MetadataInfo Map(XmlMetadataInfo xmlMetadataInfo); 
        
    }
}