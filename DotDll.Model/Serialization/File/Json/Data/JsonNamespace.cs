using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotDll.Model.Serialization.File.Json.Data
{
    public class JsonNamespace
    {
        public string Name = null!;
        
        public List<JsonType> Types = null!;
    }
}