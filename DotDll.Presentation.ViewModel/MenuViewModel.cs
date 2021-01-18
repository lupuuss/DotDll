using System.Windows.Input;
using DotDll.Logic.Metadata;
using DotDll.Logic.Navigation;
using DotDll.Presentation.ViewModel.Common;

namespace DotDll.Presentation.ViewModel
{
    public class MenuViewModel : NavigationViewModel
    {
        private readonly IMetadataService _service;
        private readonly IUserInputService _userInputService;

        private bool _pathErrorMessageShown;
        private string? _pickedFilePath;

        private ICommand? _pickFileCommand;

        public MenuViewModel(
            INavigator navigator,
            IMetadataService service,
            IUserInputService userInputService,
            RelayCommandFactory factory
        ) : base(navigator, factory)
        {
            _service = service;
            _userInputService = userInputService;
        }

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

        public string? PickedFilePath
        {
            get => _pickedFilePath;
            set
            {
                if (_pickedFilePath == value) return;

                PathErrorMessageShown = false;

                if (value == null || !_service.IsValidFileSourcePath(value))
                {
                    PathErrorMessageShown = true;
                    return;
                }

                _pickedFilePath = value;

                OnPropertyChangedAuto();
                NavigateToMetaData(value);
            }
        }

        public ICommand PickFileCommand =>
            _pickFileCommand ??= CommandFactory.CreateCommand(
                o => PickFile()
            );

        private async void PickFile()
        {
            var path = await _userInputService.PickFilePath();

            PickedFilePath = path;
        }

        private void NavigateToMetaData(string path)
        {
            Source = _service.CreateFileSource(path);
            NavigateToMetaDataCommand.Execute(null);
        }
    }
}