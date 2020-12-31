using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DotDll.Logic.Metadata.Data;
using DotDll.Logic.Metadata.Map;
using DotDll.Logic.Metadata.Sources;
using DotDll.Model.Analysis;
using DotDll.Model.Data;
using DotDll.Model.Files;
using DotDll.Model.Serialization;
using DotDll.Model.Serialization.File;
using DotDll.Model.Serialization.File.Xml;

namespace DotDll.Logic.Metadata
{
    public class MetadataService : IMetadataService
    {
        private readonly Dictionary<FileSource, MetadataInfo>
            _analyzeCache = new Dictionary<FileSource, MetadataInfo>();

        private readonly IDllAnalyzer _analyzer;

        private readonly IFilesManager _filesManager;

        private readonly IMetadataMapper _mapper;

        private readonly IMetadataSerializer _serializer;

        public MetadataService(
            IFilesManager filesManager,
            IMetadataSerializer serializer,
            IDllAnalyzer analyzer,
            IMetadataMapper mapper
        )
        {
            _filesManager = filesManager;
            _serializer = serializer;
            _analyzer = analyzer;
            _mapper = mapper;
        }

        public bool IsValidFileSourcePath(string path)
        {
            return _filesManager.FileExists(path) &&
                   (_filesManager.GetExtension(path) == ".dll" || _filesManager.GetExtension(path) == ".exe");
        }

        public Source CreateFileSource(string path)
        {
            if (IsValidFileSourcePath(path)) return new FileSource(path);

            throw new InvalidFileException();
        }

        public Task<List<Source>> GetSerializedSources()
        {
            return Task.Run(delegate
            {
                return _serializer
                    .GetAllIds()
                    .Select(id => new SerializedSource(id))
                    .Cast<Source>()
                    .ToList();
            });
        }

        public Task<MetadataDeclarations> LoadMetadata(Source source)
        {
            return source switch
            {
                FileSource fileSource => Task.Run(delegate
                {
                    var dllInfo = _analyzer.Analyze(fileSource.FilePath);

                    _analyzeCache[fileSource] = dllInfo;

                    return _mapper.Map(dllInfo);
                }),
                SerializedSource serializedSource => Task.Run(delegate
                {
                    var dllInfo = _serializer.Deserialize(serializedSource.Identifier);

                    return _mapper.Map(dllInfo);
                }),
                _ => throw new ArgumentOutOfRangeException(nameof(source))
            };
        }

        public Task<bool> SaveMetadata(Source source)
        {
            if (source is SerializedSource) throw new AlreadySerializedException();

            if (!(source is FileSource)) throw new SourceNotSerializableException();

            var fileSource = (FileSource) source;

            return Task.Run(delegate
            {
                var dllInfo = _analyzeCache.ContainsKey(fileSource)
                    ? _analyzeCache[fileSource]
                    : _analyzer.Analyze(fileSource.FilePath);

                try
                {
                    _serializer.Serialize(dllInfo);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        public static MetadataService CreateDefault()
        {
            var files = new FilesManager();
            
            return new MetadataService(
                files,
                FileMetadataSerializer.Create(".\\serialization\\", files, FileType.Xml),
                new ReflectionDllAnalyzer(Assembly.LoadFrom),
                new MetadataMapper()
            );
        }
    }

    public class InvalidFileException : Exception
    {
        public InvalidFileException() : base("File doesn't exists or has bad extension (must be dll or exe)")
        {
        }
    }

    public class AlreadySerializedException : Exception
    {
        public AlreadySerializedException() : base("Passed source that is already serialized!")
        {
        }
    }

    public class SourceNotSerializableException : Exception
    {
        public SourceNotSerializableException() : base("This type of Source cannot be serializable!")
        {
        }
    }
}