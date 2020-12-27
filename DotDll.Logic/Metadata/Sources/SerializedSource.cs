namespace DotDll.Logic.Metadata.Sources
{
    public class SerializedSource : Source
    {
        internal SerializedSource(string identifier)
        {
            Identifier = identifier;
        }

        public override string Identifier { get; }
    }
}