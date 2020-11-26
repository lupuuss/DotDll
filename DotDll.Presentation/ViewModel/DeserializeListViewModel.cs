using System.Collections.ObjectModel;
using System.Windows.Input;
using DotDll.Logic.MetaData;
using DotDll.Logic.MetaData.Sources;
using DotDll.Presentation.Navigation;

namespace DotDll.Presentation.ViewModel
{
    public class DeserializeListViewModel : NavigationViewModel
    {
        private readonly IMetaDataService _metaDataService;
        public ObservableCollection<Source> Sources { get; set; } = new ObservableCollection<Source>();

        public DeserializeListViewModel(INavigator navigator, IMetaDataService metaDataService) : base(navigator)
        {
            _metaDataService = metaDataService;

            foreach (var source in _metaDataService.GetSerializedSources())
            {
                Sources.Add(source);
            }
        }
    }
}