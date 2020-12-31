using DotDll.Model.Serialization.File.Json.Data.Base;

namespace DotDll.Model.Serialization.File.Json.Data.Members
{
    public class JsonEvent : JsonMember
    {
        
        public JsonMethod? RaiseMethod;
        
        public JsonMethod? AddMethod;
        
        public JsonMethod? RemoveMethod;
        
        public JsonType EventType = null!;
    }
}