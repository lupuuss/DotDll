using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using DotDll.Model.Data;
using DotDll.Model.Files;
using DotDll.Model.Serialization.Xml.Data;
using DotDll.Model.Serialization.Xml.Map;

namespace DotDll.Model.Serialization.Xml
{
    public class XmlMetadataSerializer : IMetadataSerializer
    {
        private readonly string _filesPath;
        private readonly IXmlMapper _mapper;
        private readonly IFilesManager _filesManager;
        private readonly DataContractSerializer _serializer;
        private readonly DataContractSerializer _indexSerializer;

        private XmlIndex? _index;

        private XmlIndex Index
        {
            get => _index ??= DeserializeIndex();
        }

        private XmlIndex DeserializeIndex()
        {
            var indexName = _filesManager.FileInPath(_filesPath, "index.xml");
            try
            {
                using var file = _filesManager.OpenFileRead(indexName);
                var index = (XmlIndex?) _indexSerializer.ReadObject(file) ?? new XmlIndex();
                
                index.Invalidate(_filesManager, _filesPath);

                return index;
            }
            catch (Exception e)
            {
                
                Debug.WriteLine(e);
                return new XmlIndex();
            }
        }

        private void SerializeIndex()
        {
            var indexName = _filesManager.FileInPath(_filesPath, "index.xml");
            try
            {
                using var stream = _filesManager.OpenFileWrite(indexName);

                _indexSerializer.WriteObject(stream, Index);
            }
            catch (Exception)
            {
                // ignore
            }
        }
        
        public XmlMetadataSerializer(string filesPath, IXmlMapper mapper, IFilesManager filesManager)
        {
            _filesPath = filesPath;
            _mapper = mapper;
            _filesManager = filesManager;
            
            _serializer = new DataContractSerializer(typeof(XmlMetadataInfo));
            _indexSerializer = new DataContractSerializer(typeof(XmlIndex));
        }
        
        public IEnumerable<string> GetAllIds()
        {
            return Index.SerializedFiles;
        }

        public MetadataInfo Deserialize(string id)
        {
            var filePath = _filesManager.FileInPath(_filesPath, id);

            using var fileStream = _filesManager.OpenFileRead(filePath);

            var xmlMetadataInfo = (XmlMetadataInfo) _serializer.ReadObject(fileStream);

            return _mapper.Map(xmlMetadataInfo);
        }

        public void Serialize(MetadataInfo metadataInfo)
        {
            MakeMainDirectoryIfNotExists();

            var guid = Guid.NewGuid();
            var name = $"{guid.ToString()}.xml";
            var filePath = _filesManager.FileInPath(_filesPath, $"{guid.ToString()}.xml");

            using var stream = _filesManager.OpenFileWrite(filePath);

            _serializer.WriteObject(stream, _mapper.Map(metadataInfo));
            
            Index.SerializedFiles.Add(name);
            SerializeIndex();
        }

        private void MakeMainDirectoryIfNotExists()
        {
            if (!_filesManager.PathExists(_filesPath))
            {
                _filesManager.MakeDirectory(_filesPath);
            }
        }
    }
}