
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using DotDll.Presentation.Navigation;
using DotDll.Presentation.ViewModel;

namespace DotDll.Presentation.View
{
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();

            var navigator = new WpfNavigator((Frame) Application.Current.MainWindow.Content);
            
            DataContext = new MenuViewModel(navigator);

            
        }
    }
}
