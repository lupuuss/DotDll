using DotDll.Model.Analysis.Data;
using DotDll.Model.Data;

namespace DotDll.Model.Analysis
{
    public interface IDllAnalyzer
    {
        DllInfo Analyze(string path);
    }
}