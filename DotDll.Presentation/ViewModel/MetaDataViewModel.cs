using System;
using DotDll.Logic.MetaData;
using DotDll.Logic.MetaData.Sources;
using DotDll.Presentation.Navigation;

namespace DotDll.Presentation.ViewModel
{
    public class MetaDataViewModel : DynamicContentViewModel
    {
        private readonly IMetaDataService _service;
        private readonly Source _source;

        private MetaData _metaData;

        public MetaDataViewModel(INavigator navigator, IMetaDataService service, Source source) : base(navigator)
        {
            _service = service;
            _source = source;

            LoadData();
        }
        
        public string MetaDataName => _source.Identifier;
        
        private async void LoadData()
        {
            IsLoading = true;
            ErrorOccured = false;
            IsContentShown = false;
            
            try
            {
                _metaData = await _service.LoadMetaData(_source);
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