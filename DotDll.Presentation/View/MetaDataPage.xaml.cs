using System.Windows;
using System.Windows.Controls;
using DotDll.Logic.MetaData.Sources;
using DotDll.Presentation.ViewModel;

namespace DotDll.Presentation.View
{
    public partial class MetaDataPage : Page
    {
        public MetaDataPage(Source source)
        {
            InitializeComponent();
            DataContext = new MetaDataViewModel(source, Application.Current.AsDotDllApp().MetaDataService);
        }
    }
}