using System.IO;

namespace DotDll.Logic.MetaData.Sources
{
    public class FileSource : Source
    {
        public override string Identifier { get; }
        
        public string FilePath { get; }
        internal FileSource(string path)
        {
            Identifier = Path.GetFileName(path);
            FilePath = path;
        }
    }
}