using System.Diagnostics;
using System.Windows.Input;
using DotDll.Presentation.Navigation;

namespace DotDll.Presentation.ViewModel
{
    public abstract class NavigationViewModel : BaseViewModel
    {

        protected readonly INavigator _navigator;

        public NavigationViewModel(INavigator navigator)
        {
            _navigator = navigator;
        }

        #region Navigation Commands
        
        private ICommand _navigateToCommand;

        public ICommand NavigateToCommand
        {
            get
            {
                return _navigateToCommand ?? (_navigateToCommand = new RelayCommand(
                    (o) => _navigator.NavigateTo((TargetView) o)
                    ));
            }
            set => _navigateToCommand = value;
        }

        private ICommand _navigateBackwards;

        public ICommand NavigateBackwardsCommand
        {
            get
            {
                return _navigateBackwards ?? (_navigateBackwards = new RelayCommand(
                    (o) => _navigator.NavigateBackward(), 
                    (o) => _navigator.CanGoBackwards()
                    ));
            }

            set => _navigateBackwards = value;
        }

        private ICommand _navigateForwards;

        public ICommand NavigateForwardsCommand
        {
            get
            {
                return _navigateForwards ?? (_navigateForwards = new RelayCommand(
                    (o) => _navigator.NavigateForwards(),
                    (o) => _navigator.CanGoForwards()
                    ));
            }
        }

        #endregion
    }
}