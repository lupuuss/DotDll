using System.Collections.Generic;
using DotDll.Model.Analysis.Data;
using DotDll.Model.Data;

namespace DotDll.Model.Serialization
{
    public interface IDllInfoSerializer
    {
        List<string> GetAllIds();

        DllInfo Deserialize(string id);

        void Serialize(DllInfo dllInfo);
    }
}