using System.Windows.Controls;
using DotDll.Presentation.ViewModel;

namespace DotDll.Presentation.View
{
    public partial class MetaDataPage : Page
    {
        public MetaDataPage()
        {
            InitializeComponent();
            DataContext = new MetaDataViewModel();
        }
    }
}