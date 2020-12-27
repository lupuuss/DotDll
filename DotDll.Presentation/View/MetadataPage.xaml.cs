using System.Windows;
using DotDll.Logic.Metadata.Sources;
using DotDll.Presentation.ViewModel.Metadata;

namespace DotDll.Presentation.View
{
    public partial class MetaDataPage
    {
        public MetaDataPage(Source source)
        {
            InitializeComponent();
            var app = Application.Current.AsDotDllApp();
            DataContext = new MetadataViewModel(app.Navigator, app.MetadataService, source);
        }
    }
}