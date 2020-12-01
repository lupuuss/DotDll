using System.IO;

namespace DotDll.Logic.MetaData.Sources
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