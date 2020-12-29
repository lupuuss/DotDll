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
        private IMapper _mapper;

        public XmlMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MetadataInfo, XmlMetadataInfo>().PreserveReferences();

                cfg.CreateMap<Member, XmlMember>().PreserveReferences();
                cfg.CreateMap<Event, XmlEvent>().IncludeBase<Member, XmlMember>().PreserveReferences();
                cfg.CreateMap<Field, XmlField>().IncludeBase<Member, XmlMember>().PreserveReferences();
                cfg.CreateMap<Method, XmlMethod>().IncludeBase<Member, XmlMember>().PreserveReferences();
                cfg.CreateMap<NestedType, XmlNestedType>().IncludeBase<Member, XmlMember>().PreserveReferences();
                cfg.CreateMap<Property, XmlProperty>().IncludeBase<Member, XmlMember>().PreserveReferences();
                cfg.CreateMap<Constructor, XmlMethod>().IncludeBase<Member, XmlMember>().PreserveReferences();
                cfg.CreateMap<Constructor, XmlConstructor>().IncludeBase<Method, XmlMethod>().PreserveReferences();

                cfg.CreateMap<Parameter, XmlParameter>().PreserveReferences();
                cfg.CreateMap<Namespace, XmlNamespace>().PreserveReferences();
                cfg.CreateMap<Type, XmlType>().PreserveReferences();
                
                cfg.CreateMap<XmlMetadataInfo, MetadataInfo>().PreserveReferences();

                cfg.CreateMap<XmlMember, Member>().PreserveReferences();
                cfg.CreateMap<XmlEvent, Event>().IncludeBase<XmlMember, Member>().PreserveReferences();
                cfg.CreateMap<XmlField, Field>().IncludeBase<XmlMember, Member>().PreserveReferences();
                cfg.CreateMap<XmlMethod, Method>().IncludeBase<XmlMember, Member>().PreserveReferences();
                cfg.CreateMap<XmlNestedType, NestedType>().IncludeBase<XmlMember, Member>().PreserveReferences();
                cfg.CreateMap<XmlProperty, Property>().IncludeBase<XmlMember, Member>().PreserveReferences();
                cfg.CreateMap<XmlMethod, Constructor>().IncludeBase<XmlMember, Member>().PreserveReferences();
                cfg.CreateMap<XmlConstructor, Constructor>().IncludeBase<Method, XmlMethod>().PreserveReferences();

                cfg.CreateMap<XmlParameter, Parameter>().PreserveReferences();
                cfg.CreateMap<XmlNamespace, Namespace>().PreserveReferences();
                cfg.CreateMap<XmlType, Type>().PreserveReferences();
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