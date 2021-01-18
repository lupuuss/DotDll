using System.Threading.Tasks;

namespace DotDll.Presentation.Model.Navigation
{
    public interface IUserInputService
    {
        Task<string?> PickFilePath();
    }
}