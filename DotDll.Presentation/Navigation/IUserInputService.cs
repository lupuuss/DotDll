using System.Threading.Tasks;

namespace DotDll.Presentation.Navigation
{
    public interface IUserInputService
    {
        Task<string?> PickFilePath();
    }
}