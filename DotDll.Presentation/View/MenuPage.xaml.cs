using System.Windows;
using System.Windows.Controls;
using DotDll.Presentation.ViewModel;

namespace DotDll.Presentation.View
{
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();

            var app = Application.Current.AsDotDllApp();

            DataContext = new MenuViewModel(app.Navigator, app.MetaDataService, app.UserInputService);
        }
    }
}