using System.Windows;
using System.Windows.Controls;
using DotDll.Presentation.ViewModel;

namespace DotDll.Presentation.View
{
    public partial class DeserializeListPage : Page
    {
        public DeserializeListPage()
        {
            InitializeComponent();
            var app = Application.Current.AsDotDllApp();
            DataContext = new DeserializeListViewModel(app.Navigator, app.MetaDataService);
        }
    }
}