using DotDll.Logic.MetaData;
using DotDll.Logic.MetaData.Sources;

namespace DotDll.Presentation.ViewModel
{
    public class MetaDataViewModel : BaseViewModel
    {
        private readonly IMetaDataService _service;
        private readonly Source _source;

        public MetaDataViewModel(Source source, IMetaDataService service)
        {
            _source = source;
            _service = service;
        }

        public string MetaDataName => _source.Identifier;
    }
}