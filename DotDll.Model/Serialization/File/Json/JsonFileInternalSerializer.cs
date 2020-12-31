using System.IO;
using System.Text;
using DotDll.Model.Data;
using AutoMapper;
using DotDll.Model.Data.Base;
using DotDll.Model.Data.Members;
using DotDll.Model.Serialization.File.Json.Data;
using DotDll.Model.Serialization.File.Json.Data.Base;
using DotDll.Model.Serialization.File.Json.Data.Members;
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
                cfg.CreateMap<MetadataInfo, JsonMetadataInfo>().ReverseMap();

                cfg.CreateMap<Member, JsonMember>()
                    .Include<Event, JsonEvent>()
                    .Include<Field, JsonField>()
                    .Include<NestedType, JsonNestedType>()
                    .Include<Property, JsonProperty>()
                    .Include<Method, JsonMethod>()
                    .Include<Constructor, JsonConstructor>()
                    .ReverseMap();


                cfg.CreateMap<Event, JsonEvent>().ReverseMap();
                cfg.CreateMap<Field, JsonField>().ReverseMap();
                cfg.CreateMap<NestedType, JsonNestedType>().ReverseMap();
                cfg.CreateMap<Property, JsonProperty>().ReverseMap();
                cfg.CreateMap<Method, JsonMethod>().ReverseMap();
                cfg.CreateMap<Constructor, JsonConstructor>().ReverseMap();

                cfg.CreateMap<Parameter, JsonParameter>().ReverseMap();
                cfg.CreateMap<Namespace, JsonNamespace>().ReverseMap();
                cfg.CreateMap<Type, JsonType>().ReverseMap();

                cfg.CreateMap<Index, JsonIndex>().ReverseMap();
                
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
            serializer.Serialize(jsonWriter, _mapper.Map<JsonIndex>(index));
        }

        public Index DeserializeIndex(Stream indexStream)
        {
            var serializer = JsonSerializer.Create(_settings);
            using var reader = new StreamReader(indexStream, Encoding.UTF8);

            return _mapper.Map<Index>((JsonIndex) serializer.Deserialize(reader, typeof(JsonIndex))!);
        }

        public void SerializeMetadata(Stream stream, MetadataInfo metadataInfo)
        {
            var serializer = JsonSerializer.Create(_settings);
            using var jsonWriter = new StreamWriter(stream);
            serializer.Serialize(jsonWriter, _mapper.Map<JsonMetadataInfo>(metadataInfo));
        }

        public MetadataInfo DeserializeMetadata(Stream stream)
        {
            var serializer = JsonSerializer.Create(_settings);
            using var reader = new StreamReader(stream, Encoding.UTF8);

            return _mapper.Map<MetadataInfo>((JsonMetadataInfo) serializer.Deserialize(reader, typeof(JsonMetadataInfo))!);
        }
    }
}