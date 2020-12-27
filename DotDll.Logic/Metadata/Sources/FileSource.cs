using System.IO;

namespace DotDll.Logic.Metadata.Sources
{
    public class FileSource : Source
    {
        internal FileSource(string path)
        {
            Identifier = Path.GetFileName(path);
            FilePath = path;
        }

        public override string Identifier { get; }

        public string FilePath { get; }
    }
}