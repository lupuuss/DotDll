using System.Windows.Input;
using DotDll.Logic.Metadata.Sources;
using DotDll.Presentation.Navigation;

namespace DotDll.Presentation.ViewModel.Common
{
    public class NavigationViewModel : BaseViewModel
    {
        private readonly INavigator _navigator;

        private ICommand? _navigateBackwardsCommand;

        private ICommand? _navigateForwardsCommand;

        private ICommand? _navigateToCommand;

        private ICommand? _navigateToMetaDataCommand;

        private Source? _source;

        public NavigationViewModel(INavigator navigator)
        {
            _navigator = navigator;
        }

        public Source? Source
        {
            get => _source;
            set
            {
                if (_source == value) return;

                _source = value;
                OnPropertyChangedAuto();
            }
        }

        public ICommand NavigateToCommand
        {
            get
            {
                return _navigateToCommand ??= new RelayCommand(
                    o => _navigator.NavigateTo((TargetView) o)
                );
            }
        }

        public ICommand NavigateBackwardsCommand
        {
            get
            {
                return _navigateBackwardsCommand ??= new RelayCommand(
                    o => _navigator.NavigateBackward(),
                    o => _navigator.CanGoBackwards()
                );
            }
        }

        public ICommand NavigateForwardsCommand
        {
            get
            {
                return _navigateForwardsCommand ??= new RelayCommand(
                    o => _navigator.NavigateForwards(),
                    o => _navigator.CanGoForwards()
                );
            }
        }

        public ICommand NavigateToMetaDataCommand
        {
            get
            {
                return _navigateToMetaDataCommand ??= new RelayCommand(
                    o => _navigator.NavigateTo(TargetView.MetaData, Source!),
                    o => Source != null
                );
            }
        }
    }
}