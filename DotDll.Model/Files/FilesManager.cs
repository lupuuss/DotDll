using System.IO;

namespace DotDll.Model.Files
{
    public class FilesManager : IFilesManager
    {
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public bool PathExists(string path)
        {
            return Directory.Exists(path);
        }

        public void MakeDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public string GetExtension(string path)
        {
            return Path.GetExtension(path);
        }

        public Stream OpenFileWrite(string path)
        {
            return File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
        }

        public Stream OpenFileRead(string path)
        {
            return File.Open(path, FileMode.Open, FileAccess.Read);
        }

        public string FileInPath(string filesPath, string filename)
        {
            return Path.Combine(filesPath, filename);
        }
    }
}