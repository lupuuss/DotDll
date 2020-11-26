using System.Windows.Input;
using DotDll.Presentation.Navigation;

namespace DotDll.Presentation.ViewModel
{
    public class NavigationViewModel : BaseViewModel
    {
        private readonly INavigator _navigator;

        private ICommand _navigateBackwards;

        private ICommand _navigateForwards;

        private ICommand _navigateToCommand;

        public NavigationViewModel(INavigator navigator)
        {
            _navigator = navigator;
        }

        public ICommand NavigateToCommand
        {
            get
            {
                return _navigateToCommand ?? (_navigateToCommand = new RelayCommand(
                    o => _navigator.NavigateTo((TargetView) o)
                ));
            }
            set => _navigateToCommand = value;
        }

        public ICommand NavigateBackwardsCommand
        {
            get
            {
                return _navigateBackwards ?? (_navigateBackwards = new RelayCommand(
                    o => _navigator.NavigateBackward(),
                    o => _navigator.CanGoBackwards()
                ));
            }

            set => _navigateBackwards = value;
        }

        public ICommand NavigateForwardsCommand
        {
            get
            {
                return _navigateForwards ?? (_navigateForwards = new RelayCommand(
                    o => _navigator.NavigateForwards(),
                    o => _navigator.CanGoForwards()
                ));
            }
        }
    }
}