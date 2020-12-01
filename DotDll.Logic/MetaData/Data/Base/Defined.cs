namespace DotDll.Logic.MetaData.Data.Base
{
    public abstract class Defined
    {
        public readonly string Definition;

        protected Defined(string definition)
        {
            Definition = definition;
        }
    }
}