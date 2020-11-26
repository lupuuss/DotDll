using System.Windows;
using System.Windows.Controls;
using DotDll.Presentation.ViewModel;
using Microsoft.Win32;

namespace DotDll.Presentation.View
{
    public partial class MenuPage : Page
    {
        private readonly MenuViewModel _viewModel;

        public MenuPage()
        {
            InitializeComponent();

            var app = Application.Current.AsDotDllApp();

            _viewModel = new MenuViewModel(app.Navigator, app.MetaDataService)
            {
                PickFileCommand = new RelayCommand(o => OpenFileDialog())
            };

            DataContext = _viewModel;
        }

        private void OpenFileDialog()
        {
            var fileDialog = new OpenFileDialog
            {
                DefaultExt = ".dll",
                Filter =
                    "DLL (*.dll)|*.dll|EXE (*.exe)|*.exe"
            };

            var result = fileDialog.ShowDialog();

            if (result != true) return;

            var filename = fileDialog.FileName;
            _viewModel.PickedFilePath = filename;
        }
    }
}