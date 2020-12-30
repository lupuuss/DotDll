using AutoMapper;
using DotDll.Model.Data;
using DotDll.Model.Data.Base;
using DotDll.Model.Data.Members;
using DotDll.Model.Serialization.Xml.Data;
using DotDll.Model.Serialization.Xml.Data.Base;
using DotDll.Model.Serialization.Xml.Data.Members;

namespace DotDll.Model.Serialization.Xml.Map
{
    public class XmlMapper : IXmlMapper
    {
        private readonly IMapper _mapper;

        public XmlMapper()
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
                
                cfg.ForAllMaps((map, exp) => exp.PreserveReferences());

                cfg.DisableConstructorMapping();
            });
            
            _mapper = config.CreateMapper();
        }
        
        public XmlMetadataInfo Map(MetadataInfo metadataInfo)
        {
            return _mapper.Map<XmlMetadataInfo>(metadataInfo);
        }
        
        public MetadataInfo Map(XmlMetadataInfo xmlMetadataInfo)
        {
            return _mapper.Map<MetadataInfo>(xmlMetadataInfo);
        }
    }
}