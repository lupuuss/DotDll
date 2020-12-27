using System;
using System.Collections.ObjectModel;
using DotDll.Logic.Metadata;
using DotDll.Logic.Metadata.Sources;
using DotDll.Presentation.Navigation;
using DotDll.Presentation.ViewModel.Common;

namespace DotDll.Presentation.ViewModel
{
    public class DeserializeListViewModel : DynamicContentViewModel
    {
        private readonly IMetadataService _metadataService;

        public DeserializeListViewModel(INavigator navigator, IMetadataService metadataService) : base(navigator)
        {
            _metadataService = metadataService;

            LoadData();
        }

        public ObservableCollection<Source> Sources { get; } = new ObservableCollection<Source>();

        private async void LoadData()
        {
            IsLoading = true;

            try
            {
                var sources = await _metadataService.GetSerializedSources();
                foreach (var source in sources) Sources.Add(source);

                IsContentShown = true;
            }
            catch (Exception)
            {
                ErrorOccured = true;
            }

            IsLoading = false;
        }
    }
}