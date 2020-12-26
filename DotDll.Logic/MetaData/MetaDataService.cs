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
    public class InvalidFileException : Exception
    {
        public InvalidFileException() : base("File doesn't exists or has bad extension (must be dll or exe)")
        {
        }
    }
    
    public class MetaDataService : IMetaDataService
    {

        private readonly IFilesManager _filesManager;

        private readonly IDllInfoSerializer _serializer;

        private readonly IDllAnalyzer _analyzer;

        private readonly IMetaDataMapper _mapper;

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
            throw new System.NotImplementedException();
        }

        public Task<bool> SaveMetaData(Source source)
        {
            throw new System.NotImplementedException();
        }
    }
}