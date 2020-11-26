namespace DotDll.Logic.MetaData.Sources
{
    public class SerializedSource : Source
    {
        public override string Identifier { get; }

        internal SerializedSource(string identifier)
        {
            Identifier = identifier;
        }
    }
}