using System.Windows;
using DotDll.Presentation.ViewModel;

namespace DotDll.Presentation.View
{
    public partial class DeserializeListPage
    {
        public DeserializeListPage()
        {
            InitializeComponent();
            var app = Application.Current.AsDotDllApp();
            DataContext = new DeserializeListViewModel(app.Navigator, app.MetadataService);
        }
    }
}