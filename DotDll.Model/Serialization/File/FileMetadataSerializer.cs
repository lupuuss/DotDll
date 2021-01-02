using System;
using System.Collections.Generic;
using AutoMapper;
using DotDll.Model.Data;
using DotDll.Model.Data.Base;
using DotDll.Model.Data.Members;
using DotDll.Model.Files;
using DotDll.Model.Serialization.File.Data;
using DotDll.Model.Serialization.File.Data.Base;
using DotDll.Model.Serialization.File.Data.Members;
using DotDll.Model.Serialization.File.Json;
using DotDll.Model.Serialization.File.Xml;
using Attribute = DotDll.Model.Data.Base.Attribute;
using Type = DotDll.Model.Data.Type;

namespace DotDll.Model.Serialization.File
{
    public enum FileType
    {
        Xml,
        Json
    }

    public class FileMetadataSerializer : IMetadataSerializer
    {
        private readonly IFilesManager _filesManager;
        private readonly string _filesPath;
        private readonly IFileInternalSerializer _internalSerializer;

        private Index? _index;

        private FileMetadataSerializer(string filesPath, IFilesManager filesManager,
            IFileInternalSerializer internalSerializer)
        {
            _filesManager = filesManager;
            _internalSerializer = internalSerializer;


            _filesPath = filesManager.FileInPath(filesPath, _internalSerializer.Extension);
        }

        private Index Index => _index ??= DeserializeIndex();

        public IEnumerable<string> GetAllIds()
        {
            Index.Invalidate(_filesManager, _filesPath, _internalSerializer.Extension);

            return Index.SerializedFiles;
        }

        public MetadataInfo Deserialize(string id)
        {
            var filePath = _filesManager.FileInPath(_filesPath, id + $".{_internalSerializer.Extension}");

            using var fileStream = _filesManager.OpenFileRead(filePath);

            return _internalSerializer.DeserializeMetadata(fileStream);
        }

        public void Serialize(MetadataInfo metadataInfo)
        {
            MakeMainDirectoryIfNotExists();

            var id = Index.NextId(metadataInfo.Name);

            var fileName = id + $".{_internalSerializer.Extension}";

            var filePath = _filesManager.FileInPath(_filesPath, fileName);

            using var stream = _filesManager.OpenFileWrite(filePath);

            _internalSerializer.SerializeMetadata(stream, metadataInfo);

            Index.SerializedFiles.Add(id);
            SerializeIndex();
        }

        private Index DeserializeIndex()
        {
            var indexName = _filesManager.FileInPath(_filesPath, $"index.{_internalSerializer.Extension}");
            try
            {
                using var file = _filesManager.OpenFileRead(indexName);
                var index = _internalSerializer.DeserializeIndex(file);

                index.Invalidate(_filesManager, _filesPath, _internalSerializer.Extension);

                return index;
            }
            catch (Exception)
            {
                return new Index();
            }
        }

        private void SerializeIndex()
        {
            var indexName = _filesManager.FileInPath(_filesPath, $"index.{_internalSerializer.Extension}");
            try
            {
                using var stream = _filesManager.OpenFileWrite(indexName);

                _internalSerializer.SerializeIndex(stream, Index);
            }
            catch (Exception)
            {
                // ignore
            }

            Index.Invalidate(_filesManager, _filesPath, _internalSerializer.Extension);
        }

        private void MakeMainDirectoryIfNotExists()
        {
            if (!_filesManager.PathExists(_filesPath)) _filesManager.MakeDirectory(_filesPath);
        }

        public static FileMetadataSerializer Create(string filesPath, IFilesManager filesManager, FileType fileType)
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

                cfg.CreateMap<Attribute, SAttribute>().ReverseMap();
                cfg.CreateMap<Parameter, SParameter>().ReverseMap();
                cfg.CreateMap<Namespace, SNamespace>().ReverseMap();
                cfg.CreateMap<Type, SType>().ReverseMap();

                cfg.CreateMap<Index, SIndex>().ReverseMap();

                cfg.ForAllMaps((map, exp) => exp.PreserveReferences());

                cfg.DisableConstructorMapping();
            });

            var mapper = config.CreateMapper();

            IFileInternalSerializer internalSerializer = fileType switch
            {
                FileType.Xml => new XmlFileInternalSerializer(mapper),
                FileType.Json => new JsonFileInternalSerializer(mapper),
                _ => throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null)
            };

            return new FileMetadataSerializer(filesPath, filesManager, internalSerializer);
        }
    }
}