using System.Windows.Input;
using DotDll.Logic.MetaData;
using DotDll.Presentation.Navigation;
using DotDll.Presentation.ViewModel.Common;

namespace DotDll.Presentation.ViewModel
{
    public class MenuViewModel : NavigationViewModel
    {
        private readonly IMetaDataService _service;
        private string _pickedFilePath;

        private bool _pathErrorMessageShown = false;

        public bool PathErrorMessageShown
        {
            get => _pathErrorMessageShown;
            set
            {
                if (_pathErrorMessageShown == value) return;

                _pathErrorMessageShown = value;
                OnPropertyChangedAuto();
            }
        }

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

                PathErrorMessageShown = false;
                
                if (!_service.IsValidFileSourcePath(value))
                {
                    PathErrorMessageShown = true;
                    return; 
                }
                
                _pickedFilePath = value;

                OnPropertyChangedAuto();
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