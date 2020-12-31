using System.IO;
using DotDll.Model.Data;

namespace DotDll.Model.Serialization.File
{
    public interface IFileInternalSerializer
    {

        public string Extension { get; }
        
        public void SerializeIndex(Stream indexStream, Index index);
        
        public Index DeserializeIndex(Stream indexStream);

        public void SerializeMetadata(Stream stream, MetadataInfo metadataInfo);

        public MetadataInfo DeserializeMetadata(Stream stream);
    }
}