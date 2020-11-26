namespace DotDll.Logic.MetaData.Sources
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