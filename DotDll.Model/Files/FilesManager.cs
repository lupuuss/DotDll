using System.IO;

namespace DotDll.Model.Files
{
    public class FilesManager : IFilesManager
    {
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public string GetExtension(string path)
        {
            return Path.GetExtension(path);
        }
    }
}