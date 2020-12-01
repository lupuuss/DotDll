using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("DotDll.Tests.Presentation")]
namespace DotDll.Logic.MetaData.Sources
{
    public abstract class Source
    {
        public abstract string Identifier { get; }
    }
}