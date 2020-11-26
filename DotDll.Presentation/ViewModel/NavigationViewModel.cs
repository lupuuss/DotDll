using System.Windows.Input;
using DotDll.Presentation.Navigation;

namespace DotDll.Presentation.ViewModel
{
    public class NavigationViewModel : BaseViewModel
    {
        protected readonly INavigator Navigator;

        private ICommand _navigateBackwards;

        private ICommand _navigateForwards;

        private ICommand _navigateToCommand;

        public NavigationViewModel(INavigator navigator)
        {
            Navigator = navigator;
        }

        public ICommand NavigateToCommand
        {
            get
            {
                return _navigateToCommand ?? (_navigateToCommand = new RelayCommand(
                    o => Navigator.NavigateTo((TargetView) o)
                ));
            }
            set => _navigateToCommand = value;
        }

        public ICommand NavigateBackwardsCommand
        {
            get
            {
                return _navigateBackwards ?? (_navigateBackwards = new RelayCommand(
                    o => Navigator.NavigateBackward(),
                    o => Navigator.CanGoBackwards()
                ));
            }

            set => _navigateBackwards = value;
        }

        public ICommand NavigateForwardsCommand
        {
            get
            {
                return _navigateForwards ?? (_navigateForwards = new RelayCommand(
                    o => Navigator.NavigateForwards(),
                    o => Navigator.CanGoForwards()
                ));
            }
        }
    }
}