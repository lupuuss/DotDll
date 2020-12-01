using System.Windows;
using System.Windows.Controls;
using DotDll.Presentation.ViewModel;
using DotDll.Presentation.ViewModel.Common;
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

            _viewModel = new MenuViewModel(app.Navigator, app.MetaDataService, app.UserInputService);

            DataContext = _viewModel;
        }
        
    }
}