using System.Windows;
using System.Windows.Controls;
using DotDll.Logic.MetaData.Sources;
using DotDll.Presentation.ViewModel;
using DotDll.Presentation.ViewModel.MetaData;

namespace DotDll.Presentation.View
{
    public partial class MetaDataPage : Page
    {
        public MetaDataPage(Source source)
        {
            InitializeComponent();
            var app = Application.Current.AsDotDllApp();
            DataContext = new MetaDataViewModel(app.Navigator, app.MetaDataService, source);
        }
    }
}