namespace DotDll.Model.Serialization.File.Json.Data.Base
{
    
    public abstract class JsonMember
    {
        public string Name = null!;
        
        public int AccessLevel;
        
        public bool IsStatic;
        
        public bool IsAbstract;
    }
}