using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using DotDll.Model.Data;
using DotDll.Model.Files;
using DotDll.Model.Serialization;
using DotDll.Model.Serialization.Xml;
using DotDll.Model.Serialization.Xml.Map;
using Moq;
using NUnit.Framework;

namespace DotDll.Tests.Model.Serialization.Xml
{
    [TestFixture]
    public class XmlMetadataSerializerTest
    {

        private IMetadataSerializer _serializer;
        private Mock<IFilesManager> _filesManager;
        private MemoryStream _indexStream;
        private Dictionary<string, MemoryStream> _streams = new Dictionary<string, MemoryStream>();

        [SetUp]
        public void SetUp()
        {
            _filesManager = new Mock<IFilesManager>();

            _filesManager
                .Setup(f => f.PathExists(It.IsAny<string>()))
                .Returns(true);

            _filesManager
                .Setup(f => f.FileInPath(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>(Path.Combine);

            _indexStream = new MemoryStream();
            
            _filesManager
                .Setup(f => f.OpenFileRead("index.xml"))
                .Returns(_indexStream);
                        
            _filesManager
                .Setup(f => f.OpenFileWrite("index.xml"))
                .Returns(_indexStream);

            _filesManager
                .Setup(f => f.OpenFileRead(It.IsNotIn("index.xml")))
                .Returns<string>((path) =>
                {
                    if (!_streams.ContainsKey(path)) _streams[path] = new MemoryStream();
                    
                    return _streams[path];
                });
            
            _filesManager
                .Setup(f => f.OpenFileWrite(It.IsNotIn("index.xml")))
                .Returns<string>((path) =>
                {
                    if (!_streams.ContainsKey(path)) _streams[path] = new MemoryStream();
                    
                    return _streams[path];
                });



            _serializer = new XmlMetadataSerializer("", new XmlMapper(), _filesManager.Object);
        }

        [Test]
        public void Test1()
        {
            var x = new MetadataInfo("Dada");
            
            
            _serializer.Serialize(x);
            
            Debug.WriteLine(string.Join(", ",_streams.Keys));
            
            Debug.WriteLine(System.Text.Encoding.UTF8.GetString(_streams["Dada_0.xml"].ToArray()));
        }
    }
}