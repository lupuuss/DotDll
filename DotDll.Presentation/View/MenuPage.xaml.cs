using System.Windows;
using DotDll.Presentation.ViewModel;

namespace DotDll.Presentation.View
{
    public partial class MenuPage
    {
        public MenuPage()
        {
            InitializeComponent();

            var app = Application.Current.AsDotDllApp();

            DataContext = new MenuViewModel(app.Navigator, app.MetadataServiceImpl, app.UserInputService);
        }
    }
}