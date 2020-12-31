using DotDll.Model.Serialization.File.Json.Data.Base;

namespace DotDll.Model.Serialization.File.Json.Data.Members
{
    public class JsonProperty : JsonMember
    {
        public JsonMethod? Getter;
        
        public JsonMethod? Setter;
        
        public bool CanRead;
        
        public bool CanWrite;
    }
}