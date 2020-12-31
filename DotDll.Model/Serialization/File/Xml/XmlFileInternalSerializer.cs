using System.IO;
using System.Runtime.Serialization;
using AutoMapper;
using DotDll.Model.Data;
using DotDll.Model.Serialization.File.Data;

namespace DotDll.Model.Serialization.File.Xml
{
    public class XmlFileInternalSerializer : IFileInternalSerializer
    {
        private readonly IMapper _mapper;
        
        public XmlFileInternalSerializer(IMapper mapper)
        {
            _mapper = mapper;
        }

        public string Extension { get; } = "xml";

        public void SerializeIndex(Stream indexStream, Index index)
        {
            var dataSerializer = new DataContractSerializer(typeof(SIndex));
            
            dataSerializer.WriteObject(indexStream, _mapper.Map<SIndex>(index));
        }

        public Index DeserializeIndex(Stream indexStream)
        {
            var dataSerializer = new DataContractSerializer(typeof(SIndex));
            
            return _mapper.Map<Index>((SIndex) dataSerializer.ReadObject(indexStream));
        }

        public void SerializeMetadata(Stream stream, MetadataInfo metadataInfo)
        {
            var dataSerializer = new DataContractSerializer(typeof(SMetadataInfo));
            
            dataSerializer.WriteObject(stream, _mapper.Map<SMetadataInfo>(metadataInfo));
        }

        public MetadataInfo DeserializeMetadata(Stream stream)
        {
            var dataSerializer = new DataContractSerializer(typeof(SMetadataInfo));
            
            return _mapper.Map<MetadataInfo>((SMetadataInfo) dataSerializer.ReadObject(stream));
            
        }
    }
}