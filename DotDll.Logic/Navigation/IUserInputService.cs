using System.Threading.Tasks;

namespace DotDll.Logic.Navigation
{
    public interface IUserInputService
    {
        Task<string?> PickFilePath();
    }
}