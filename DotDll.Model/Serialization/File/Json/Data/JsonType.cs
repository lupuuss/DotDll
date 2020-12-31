using System.Collections.Generic;
using System.Runtime.Serialization;
using DotDll.Model.Serialization.File.Json.Data.Base;
using Newtonsoft.Json;

namespace DotDll.Model.Serialization.File.Json.Data
{
    public class JsonType
    {
        public string Name = null!;
        
        public int Access;
        
        public int TypeKind;
        
        public bool IsSealed;
        
        public bool IsAbstract;
        
        public bool IsStatic;
        
        public List<JsonMember> Members = null!;
        
        public List<JsonType> GenericArguments = null!;
        
        public List<JsonType> GenericConstraints = null!;

        public List<JsonType> BaseTypes = null!;
    }
}