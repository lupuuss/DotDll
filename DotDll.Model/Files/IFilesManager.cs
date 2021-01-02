using System.IO;

namespace DotDll.Model.Files
{
    public interface IFilesManager
    {
        bool FileExists(string path);

        bool PathExists(string path);

        void MakeDirectory(string path);

        string GetExtension(string path);

        public Stream OpenFileWrite(string path);

        public Stream OpenFileRead(string path);

        string FileInPath(string filesPath, string filename);
    }
}