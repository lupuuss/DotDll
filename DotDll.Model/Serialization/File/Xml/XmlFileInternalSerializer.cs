using System.IO;
using System.Runtime.Serialization;
using AutoMapper;
using DotDll.Model.Data;
using DotDll.Model.Data.Base;
using DotDll.Model.Data.Members;
using DotDll.Model.Serialization.File.Data;
using DotDll.Model.Serialization.File.Data.Base;
using DotDll.Model.Serialization.File.Data.Members;

namespace DotDll.Model.Serialization.File.Xml
{
    public class XmlFileInternalSerializer : IFileInternalSerializer
    {
        private readonly IMapper _mapper;
        
        public XmlFileInternalSerializer()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MetadataInfo, SMetadataInfo>().ReverseMap();

                cfg.CreateMap<Member, SMember>()
                    .Include<Event, SEvent>()
                    .Include<Field, SField>()
                    .Include<NestedType, SNestedType>()
                    .Include<Property, SProperty>()
                    .Include<Method, SMethod>()
                    .Include<Constructor, SConstructor>()
                    .ReverseMap();


                cfg.CreateMap<Event, SEvent>().ReverseMap();
                cfg.CreateMap<Field, SField>().ReverseMap();
                cfg.CreateMap<NestedType, SNestedType>().ReverseMap();
                cfg.CreateMap<Property, SProperty>().ReverseMap();
                cfg.CreateMap<Method, SMethod>().ReverseMap();
                cfg.CreateMap<Constructor, SConstructor>().ReverseMap();

                cfg.CreateMap<Parameter, SParameter>().ReverseMap();
                cfg.CreateMap<Namespace, SNamespace>().ReverseMap();
                cfg.CreateMap<Type, SType>().ReverseMap();

                cfg.CreateMap<Index, SIndex>().ReverseMap();
                
                cfg.ForAllMaps((map, exp) => exp.PreserveReferences());

                cfg.DisableConstructorMapping();
            });
            
            _mapper = config.CreateMapper();
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