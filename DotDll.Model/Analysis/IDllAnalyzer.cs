using DotDll.Model.Analysis.Data;

namespace DotDll.Model.Analysis
{
    public interface IDllAnalyzer
    {
        DllInfo Analyze(string path);
    }
}