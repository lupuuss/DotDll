using System;
using System.Collections.ObjectModel;
using DotDll.Logic.MetaData;
using DotDll.Logic.MetaData.Sources;
using DotDll.Presentation.Navigation;
using DotDll.Presentation.ViewModel.Common;

namespace DotDll.Presentation.ViewModel
{
    public class DeserializeListViewModel : DynamicContentViewModel
    {
        private readonly IMetaDataService _metaDataService;

        public DeserializeListViewModel(INavigator navigator, IMetaDataService metaDataService) : base(navigator)
        {
            _metaDataService = metaDataService;

            LoadData();
        }

        public ObservableCollection<Source> Sources { get; set; } = new ObservableCollection<Source>();

        private async void LoadData()
        {
            IsLoading = true;

            try
            {
                var sources = await _metaDataService.GetSerializedSources();
                foreach (var source in sources)
                {
                    Sources.Add(source);
                }

                IsContentShown = true;
            }
            catch (Exception e)
            {
                ErrorOccured = true;
            }

            IsLoading = false;
        }
    }
}