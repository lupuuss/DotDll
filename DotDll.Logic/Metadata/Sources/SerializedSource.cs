namespace DotDll.Logic.Metadata.Sources
{
    public class SerializedSource : Source
    {
        public SerializedSource(string identifier)
        {
            Identifier = identifier;
        }

        public override string Identifier { get; }
    }
}