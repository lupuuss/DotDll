namespace DotDll.Logic.MetaData.Sources
{
    public class FileSource : Source
    {
        public override string Identifier { get; }

        internal FileSource(string path)
        {
            Identifier = path;
        }
    }
}