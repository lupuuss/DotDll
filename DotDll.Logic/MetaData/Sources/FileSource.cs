namespace DotDll.Logic.MetaData.Sources
{
    public class FileSource : Source
    {
        internal FileSource(string path)
        {
            Identifier = path;
        }

        public override string Identifier { get; }
    }
}