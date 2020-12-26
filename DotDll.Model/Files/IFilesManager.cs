namespace DotDll.Model.Files
{
    public interface IFilesManager
    {
        bool FileExists(string path);

        string GetExtension(string path);
    }
}