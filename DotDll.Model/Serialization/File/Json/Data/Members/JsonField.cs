using DotDll.Model.Serialization.File.Json.Data.Base;

namespace DotDll.Model.Serialization.File.Json.Data.Members 
{
    public class JsonField : JsonMember
    {
        public int FieldConstraint;
        
        public JsonType ReturnType = null!;
    }
}