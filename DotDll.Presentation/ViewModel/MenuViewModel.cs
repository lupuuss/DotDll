﻿using System.Windows.Input;
using DotDll.Logic.MetaData;
using DotDll.Presentation.Navigation;
using DotDll.Presentation.ViewModel.Common;

namespace DotDll.Presentation.ViewModel
{
    public class MenuViewModel : NavigationViewModel
    {
        private readonly IMetaDataService _service;
        private readonly IUserInputService _userInputService;

        private bool _pathErrorMessageShown;
        private string _pickedFilePath;

        private ICommand _pickFileCommand;

        public MenuViewModel(
            INavigator navigator,
            IMetaDataService service,
            IUserInputService userInputService
        ) : base(navigator)
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

        public ICommand PickFileCommand =>
            _pickFileCommand ?? (_pickFileCommand = new RelayCommand(
                o => PickFile()
            ));

        private async void PickFile()
        {
            var path = await _userInputService.PickFilePath();

            PickedFilePath = path;
        }

        private void NavigateToMetaData()
        {
            Source = _service.CreateFileSource(PickedFilePath);
            NavigateToMetaDataCommand.Execute(null);
        }
    }
}