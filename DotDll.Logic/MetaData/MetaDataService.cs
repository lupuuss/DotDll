using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotDll.Logic.MetaData.Data;
using DotDll.Logic.MetaData.Map;
using DotDll.Logic.MetaData.Sources;
using DotDll.Model.Analysis;
using DotDll.Model.Data;
using DotDll.Model.Files;
using DotDll.Model.Serialization;

namespace DotDll.Logic.MetaData
{

    public class MetaDataService : IMetaDataService
    {

        private readonly IFilesManager _filesManager;

        private readonly IDllInfoSerializer _serializer;

        private readonly IDllAnalyzer _analyzer;

        private readonly IMetaDataMapper _mapper;

        private readonly Dictionary<FileSource, DllInfo> _analyzeCache = new Dictionary<FileSource, DllInfo>();

        public MetaDataService(
            IFilesManager filesManager, 
            IDllInfoSerializer serializer, 
            IDllAnalyzer analyzer, 
            IMetaDataMapper mapper
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
                   (_filesManager.GetExtension(path) == "dll" || _filesManager.GetExtension(path) == "exe");
        }

        public Source CreateFileSource(string path)
        {
            if (IsValidFileSourcePath(path))
            {
                return new FileSource(path);
            }

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

        public Task<MetaDataObject> LoadMetaData(Source source)
        {
            switch (source)
            {
                case FileSource fileSource:

                    return Task.Run(delegate
                    {
                        var dllInfo = _analyzer.Analyze(fileSource.FilePath);

                        _analyzeCache[fileSource] = dllInfo;
                        
                        return _mapper.Map(dllInfo);
                    });
                    
                case SerializedSource serializedSource:
                    
                    return Task.Run(delegate
                    {
                        var dllInfo = _serializer.Deserialize(serializedSource.Identifier);

                        return _mapper.Map(dllInfo);
                    });
                default:
                    throw new ArgumentOutOfRangeException(nameof(source));
            }
            
        }

        public Task<bool> SaveMetaData(Source source)
        {
            if (source is SerializedSource)
            {
                throw new AlreadySerializedException();
            }

            if (!(source is FileSource))
            {
                throw new SourceNotSerializableException();
            }

            var fileSource = (FileSource) source;
            
            return Task.Run(delegate
            {
                var dllInfo = _analyzeCache.ContainsKey(fileSource) ? _analyzeCache[fileSource] : _analyzer.Analyze(fileSource.FilePath);

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