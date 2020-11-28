using System.Security.AccessControl;
using System.Windows.Input;
using DotDll.Logic.MetaData;
using DotDll.Presentation.Navigation;

namespace DotDll.Presentation.ViewModel
{
    public class MenuViewModel : NavigationViewModel
    {
        private readonly IMetaDataService _service;
        private string _pickedFilePath;

        public MenuViewModel(INavigator navigator, IMetaDataService service) : base(navigator)
        {
            _service = service;
        }

        public string PickedFilePath
        {
            get => _pickedFilePath;
            set
            {
                if (_pickedFilePath == value) return;

                if (!_service.IsValidFileSourcePath(value))
                    // TODO
                    return;

                _pickedFilePath = value;

                OnPropertyChanged("PickedFilePath");
                NavigateToMetaData();
            }
        }

        public ICommand PickFileCommand { get; set; }

        private void NavigateToMetaData()
        {
            Source = _service.CreateFileSource(PickedFilePath);
            NavigateToMetaDataCommand.Execute(null);
        }
    }
}