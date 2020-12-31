using System.Collections.Generic;
using DotDll.Model.Serialization.File.Json.Data.Base;

namespace DotDll.Model.Serialization.File.Json.Data.Members
{

    public class JsonMethod : JsonMember
    {
        public bool IsVirtual;
        
        public bool IsSealed;
        
        public JsonType ReturnType = null!;
        
        public List<JsonParameter> Parameters = null!;
        
        public List<JsonType> GenericArguments = null!;
    }
}