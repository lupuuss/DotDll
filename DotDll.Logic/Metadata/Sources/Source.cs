using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DotDll.Tests.Presentation.ViewModel")]

namespace DotDll.Logic.Metadata.Sources
{
    public abstract class Source
    {
        public abstract string Identifier { get; }
    }
}