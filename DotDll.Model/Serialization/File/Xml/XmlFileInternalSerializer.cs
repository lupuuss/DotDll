using System.IO;
using System.Runtime.Serialization;
using AutoMapper;
using DotDll.Model.Data;
using DotDll.Model.Data.Base;
using DotDll.Model.Data.Members;
using DotDll.Model.Serialization.File.Xml.Data;
using DotDll.Model.Serialization.File.Xml.Data.Base;
using DotDll.Model.Serialization.File.Xml.Data.Members;

namespace DotDll.Model.Serialization.File.Xml
{
    public class XmlFileInternalSerializer : IFileInternalSerializer
    {
        private readonly IMapper _mapper;
        
        public XmlFileInternalSerializer()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MetadataInfo, XmlMetadataInfo>().ReverseMap();

                cfg.CreateMap<Member, XmlMember>()
                    .Include<Event, XmlEvent>()
                    .Include<Field, XmlField>()
                    .Include<NestedType, XmlNestedType>()
                    .Include<Property, XmlProperty>()
                    .Include<Method, XmlMethod>()
                    .Include<Constructor, XmlConstructor>()
                    .ReverseMap();


                cfg.CreateMap<Event, XmlEvent>().ReverseMap();
                cfg.CreateMap<Field, XmlField>().ReverseMap();
                cfg.CreateMap<NestedType, XmlNestedType>().ReverseMap();
                cfg.CreateMap<Property, XmlProperty>().ReverseMap();
                cfg.CreateMap<Method, XmlMethod>().ReverseMap();
                cfg.CreateMap<Constructor, XmlConstructor>().ReverseMap();

                cfg.CreateMap<Parameter, XmlParameter>().ReverseMap();
                cfg.CreateMap<Namespace, XmlNamespace>().ReverseMap();
                cfg.CreateMap<Type, XmlType>().ReverseMap();

                cfg.CreateMap<Index, XmlIndex>().ReverseMap();
                
                cfg.ForAllMaps((map, exp) => exp.PreserveReferences());

                cfg.DisableConstructorMapping();
            });
            
            _mapper = config.CreateMapper();
        }

        public string Extension { get; } = "xml";

        public void SerializeIndex(Stream indexStream, Index index)
        {
            var dataSerializer = new DataContractSerializer(typeof(XmlIndex));
            
            dataSerializer.WriteObject(indexStream, _mapper.Map<XmlIndex>(index));
        }

        public Index DeserializeIndex(Stream indexStream)
        {
            var dataSerializer = new DataContractSerializer(typeof(XmlIndex));
            
            return _mapper.Map<Index>((XmlIndex) dataSerializer.ReadObject(indexStream));
        }

        public void SerializeMetadata(Stream stream, MetadataInfo metadataInfo)
        {
            var dataSerializer = new DataContractSerializer(typeof(XmlMetadataInfo));
            
            dataSerializer.WriteObject(stream, _mapper.Map<XmlMetadataInfo>(metadataInfo));
        }

        public MetadataInfo DeserializeMetadata(Stream stream)
        {
            var dataSerializer = new DataContractSerializer(typeof(XmlMetadataInfo));
            
            return _mapper.Map<MetadataInfo>((XmlMetadataInfo) dataSerializer.ReadObject(stream));
            
        }
    }
}