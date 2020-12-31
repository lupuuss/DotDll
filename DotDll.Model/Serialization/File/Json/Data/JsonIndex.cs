using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotDll.Model.Serialization.File.Json.Data
{
    public class JsonIndex
    {
        public SortedSet<string> SerializedFiles { get; set; } = new SortedSet<string>();
    }
}