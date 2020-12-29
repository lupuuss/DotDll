using System.Threading.Tasks;
using Microsoft.Win32;

namespace DotDll.Presentation.Navigation
{
    public class WpfUserInputService : IUserInputService
    {
        public Task<string?> PickFilePath()
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = "Assembly (*.exe;*.dll)|*.exe;*.dll"
            };

            var result = fileDialog.ShowDialog();

            if (result != true) return Task.FromResult<string?>(null);

            var filename = fileDialog.FileName;
            return Task.FromResult<string?>(filename);
        }
    }
}