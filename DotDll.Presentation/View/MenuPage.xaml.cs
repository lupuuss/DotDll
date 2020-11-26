using System;
using System.Windows;
using System.Windows.Controls;
using DotDll.Presentation.ViewModel;

namespace DotDll.Presentation.View
{
    public partial class MenuPage : Page
    {

        private MenuViewModel _viewModel;
        public MenuPage()
        {
            InitializeComponent();

            _viewModel = new MenuViewModel(Application.Current.AsDotDllApp().Navigator)
            {
                PickFileCommand = new RelayCommand((o) => OpenFileDialog())
            };
            
            DataContext = _viewModel;

        }

        private void OpenFileDialog()
        {
            var fileDialog = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".dll",
                Filter =
                    "DLL (*.dll)|*.dll|EXE (*.exe)|*.exe"
            };

            bool? result = fileDialog.ShowDialog();

            if (result != true) return;
            
            var filename = fileDialog.FileName;
            _viewModel.PickedFilePath = filename;
        }
    }
}
