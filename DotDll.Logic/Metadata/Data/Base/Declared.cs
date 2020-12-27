namespace DotDll.Logic.Metadata.Data.Base
{
    public abstract class Declared
    {
        public readonly string Declaration;

        protected Declared(string declaration)
        {
            Declaration = declaration;
        }
    }
}