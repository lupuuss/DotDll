using System.Collections.Generic;
using DotDll.Model.Analysis.Data;

namespace DotDll.Model.Serialization
{
    public interface IDllInfoSerializer
    {
        List<string> GetAllIds();

        DllInfo Deserialize(string id);

        void Serialize(DllInfo dllInfo);
    }
}