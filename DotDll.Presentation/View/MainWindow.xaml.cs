using System.Windows;
using System.Windows.Controls;
using DotDll.Presentation.View.Navigation;
using DotDll.Presentation.ViewModel.Common;

namespace DotDll.Presentation.View
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            var frame = (Frame) ((Grid) Content).FindName("MainFrame");
            var navigator = new WpfNavigator(frame);

            // initializes navigator for other classes.
            Application.Current.AsDotDllApp().Navigator = navigator;

            DataContext = new NavigationViewModel(navigator);
        }
    }
}