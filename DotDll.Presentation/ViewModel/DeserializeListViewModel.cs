using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
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

        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading == value) return;

                _isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }
        
        public DeserializeListViewModel(INavigator navigator, IMetaDataService metaDataService) : base(navigator)
        {
            _metaDataService = metaDataService;

            LoadData();
        }

        private async void LoadData()
        {
            IsLoading = true;
            
            var sources = await _metaDataService.GetSerializedSources();
            foreach (var source in sources)
            {
                Sources.Add(source);
            }
            
            IsLoading = false;
        }
    }
}