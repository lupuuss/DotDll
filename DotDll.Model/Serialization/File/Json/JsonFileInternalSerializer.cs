using System.IO;
using System.Text;
using DotDll.Model.Data;
using AutoMapper;
using DotDll.Model.Data.Base;
using DotDll.Model.Data.Members;
using DotDll.Model.Serialization.File.Data;
using DotDll.Model.Serialization.File.Data.Base;
using DotDll.Model.Serialization.File.Data.Members;
using Newtonsoft.Json;

namespace DotDll.Model.Serialization.File.Json
{
    public class JsonFileInternalSerializer : IFileInternalSerializer
    {
        private readonly IMapper _mapper;

        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings()
        {
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            TypeNameHandling = TypeNameHandling.Auto
        };

        public JsonFileInternalSerializer()
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
        
        public string Extension { get; } = "json";
        public void SerializeIndex(Stream indexStream, Index index)
        {
            var serializer = JsonSerializer.Create(_settings);
            using var jsonWriter = new StreamWriter(indexStream);
            serializer.Serialize(jsonWriter, _mapper.Map<SIndex>(index));
        }

        public Index DeserializeIndex(Stream indexStream)
        {
            var serializer = JsonSerializer.Create(_settings);
            using var reader = new StreamReader(indexStream, Encoding.UTF8);

            return _mapper.Map<Index>((SIndex) serializer.Deserialize(reader, typeof(SIndex))!);
        }

        public void SerializeMetadata(Stream stream, MetadataInfo metadataInfo)
        {
            var serializer = JsonSerializer.Create(_settings);
            using var jsonWriter = new StreamWriter(stream);
            serializer.Serialize(jsonWriter, _mapper.Map<SMetadataInfo>(metadataInfo));
        }

        public MetadataInfo DeserializeMetadata(Stream stream)
        {
            var serializer = JsonSerializer.Create(_settings);
            using var reader = new StreamReader(stream, Encoding.UTF8);

            return _mapper.Map<MetadataInfo>((SMetadataInfo) serializer.Deserialize(reader, typeof(SMetadataInfo))!);
        }
    }
}