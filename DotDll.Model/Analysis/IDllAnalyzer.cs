using DotDll.Model.Data;

namespace DotDll.Model.Analysis
{
    public interface IDllAnalyzer
    {
        MetadataInfo Analyze(string path);
    }
}