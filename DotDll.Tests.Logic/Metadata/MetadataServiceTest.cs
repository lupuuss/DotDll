using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DotDll.Logic.Metadata;
using DotDll.Logic.Metadata.Data;
using DotDll.Logic.Metadata.Map;
using DotDll.Logic.Metadata.Sources;
using DotDll.Model.Analysis;
using DotDll.Model.Data;
using DotDll.Model.Files;
using DotDll.Model.Serialization;
using Moq;
using NUnit.Framework;

namespace DotDll.Tests.Logic.Metadata
{
    [TestFixture]
    public class MetadataServiceTest
    {

        private Mock<IDllAnalyzer> _analyzerMock;
        private Mock<IFilesManager> _filesManager;
        private Mock<IMetadataMapper> _metadataMapper;
        private Mock<IMetadataSerializer> _metadataSerializer;

        private readonly List<string> _allIds = new List<string>()
        {
            "id0",
            "id1",
            "id2"
        };
        

        private MetadataService _service;

        [SetUp]
        public void SetUp()
        {
            _analyzerMock = new Mock<IDllAnalyzer>();
            _filesManager = new Mock<IFilesManager>();
            _metadataMapper = new Mock<IMetadataMapper>();
            _metadataSerializer = new Mock<IMetadataSerializer>();
            
            _analyzerMock.Setup(a => a.Analyze(It.IsAny<string>()))
                .Returns(new MetadataInfo("Test"));

            _filesManager.Setup(a => a.FileExists(It.IsAny<string>()))
                .Returns(true);

            _filesManager.Setup(a => a.GetExtension(It.IsAny<string>()))
                .Returns<string>(Path.GetExtension);

            _metadataMapper.Setup(a => a.Map(It.IsAny<MetadataInfo>()))
                .Returns(new MetadataDeclarations("Test", new List<DNamespace>()));


            _metadataSerializer.Setup(a => a.GetAllIds())
                .Returns(_allIds);

            _metadataSerializer.Setup(a => a.Deserialize(It.IsAny<string>()))
                .Returns(new MetadataInfo("Test1"));
        }

        private void InitService()
        {
            _service = new MetadataService(
                _filesManager.Object,
                _metadataSerializer.Object,
                _analyzerMock.Object,
                _metadataMapper.Object
            );
        }

        [TestCase("test/path.dll")]
        [TestCase("test/path.exe")]
        public void IsValidFileSourcePath_FileExistsAndProperExtension_ReturnsTrue(string path)
        {
            InitService();

            var actual = _service.IsValidFileSourcePath(path);

            Assert.True(actual);

            _filesManager.Verify(f => f.FileExists(path), Times.Once);

            _filesManager.Verify(f => f.GetExtension(path), Times.AtLeastOnce);
        }


        [Test]
        public void IsValidFileSourcePath_NotProperExtension_ReturnsFalse()
        {
            InitService();

            var actual = _service.IsValidFileSourcePath("test/path.txt");

            Assert.False(actual);

            _filesManager.Verify(f => f.FileExists("test/path.txt"), Times.Once);

            _filesManager.Verify(f => f.GetExtension("test/path.txt"), Times.AtLeastOnce);
        }

        [Test]
        public void IsValidFileSourcePath_FileDoesNotExist_ReturnFalse()
        {
            _filesManager.Setup(a => a.FileExists(It.IsAny<string>()))
                .Returns(false);

            InitService();

            var actual = _service.IsValidFileSourcePath("test/path.dll");

            Assert.False(actual);

            _filesManager.Verify(f => f.FileExists("test/path.dll"), Times.Once);

        }

        [Test]
        public void CreateFileSource_InvalidFile_ThrowsException()
        {
            _filesManager.Setup(a => a.FileExists(It.IsAny<string>()))
                .Returns(false);

            InitService();

            Assert.Catch<InvalidFileException>(() => _service.CreateFileSource("TestPath.txt"));

        }

        [Test]
        public void CreateFileSource_ValidFile_ReturnsValidFileSource()
        {
            _filesManager.Setup(a => a.FileExists(It.IsAny<string>()))
                .Returns(true);

            InitService();

            var actual = _service.CreateFileSource("TestPath.dll");

            Assert.IsInstanceOf<FileSource>(actual);
            Assert.AreEqual("TestPath.dll", ((FileSource) actual).FilePath);

        }

        [Test]
        public void GetSerializedSources_Always_ReturnsValidIdList()
        {

            InitService();

            var actual = _service.GetSerializedSources().Result;

            _metadataSerializer.Verify(a => a.GetAllIds(), Times.Once);

            CollectionAssert.AreEqual(_allIds, actual.Select(s => s.Identifier));
        }

        [Test]
        public void LoadMetaData_FileSource_ReturnsDataFromAnalyzer() 
        {
            InitService();

            var actual = _service.LoadMetadata(new FileSource("Test/path.dll")).Result;

            Assert.NotNull(actual);
            
            _analyzerMock.Verify(
                a => a.Analyze(It.IsAny<string>()),
                Times.Once
                );
            
            _metadataMapper.Verify(
                a => a.Map(It.IsAny<MetadataInfo>()),
                Times.Once
                );

        }
        
        [Test]
        public void LoadMetaData_SerializedSource_ReturnsDataFromSerializer()
        {
            InitService();

            var actual = _service.LoadMetadata(new SerializedSource("id0")).Result;

            Assert.NotNull(actual);
            
            _metadataSerializer.Verify(
                s => s.Deserialize("id0")
                );
            
            _metadataMapper.Verify(
                a => a.Map(It.IsAny<MetadataInfo>()),
                Times.Once
                );

        }
        
        [Test]
        public void LoadMetaData_NullSource_ThrowsException() 
        {
            InitService();

            Assert.Catch<ArgumentOutOfRangeException>(() => _service.LoadMetadata(null!));
            
        }

        [Test]
        public void SaveMetadata_SerializedSource_ThrowsException()
        {
            InitService();

            Assert.Catch<AlreadySerializedException>(
                () => _service.SaveMetadata(new SerializedSource("test/path.dll"))
                ); 
        }

        [Test]
        public void SaveMetadata_NotFileSource_ThrowsException()
        {
            InitService();

            Assert.Catch<SourceNotSerializableException>(() => _service.SaveMetadata(null!));
            
        }

        [Test]
        public void SaveMetadata_ValidSource_ReturnsTrue()
        {
            InitService();

            var actual = _service.SaveMetadata(new FileSource("test1")).Result;
            
            Assert.True(actual);
            
            _metadataSerializer.Verify(
                a => a.Serialize(It.IsAny<MetadataInfo>()), Times.Once
                );
            
        }
        

        [Test]
        public void SaveMetadata_ValidSourceSerializationError_ReturnFalse()
        {
            _metadataSerializer.Setup(a => a.Serialize(It.IsAny<MetadataInfo>()))
                .Throws(new Exception());
            
            InitService();

            var actual = _service.SaveMetadata(new FileSource("test/path.dll")).Result;
            
            _metadataSerializer.Verify(
                s => s.Serialize(It.IsAny<MetadataInfo>()), Times.Once
                );
            
            Assert.False(actual);
        }
    }
}