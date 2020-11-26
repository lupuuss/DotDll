namespace DotDll.Presentation.ViewModel
{
    public class MetaDataViewModel : BaseViewModel
    {

        private string _metaDataName = "Some.dll";
        
        public string MetaDataName
        {
            get => _metaDataName;
            set
            {
                if (_metaDataName == value) return;

                _metaDataName = value;
                OnPropertyChanged("MetaDataName");
            }
        }
    }
}