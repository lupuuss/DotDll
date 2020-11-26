using System.Windows.Input;
using DotDll.Logic.MetaData.Sources;
using DotDll.Presentation.Navigation;

namespace DotDll.Presentation.ViewModel
{
    public class NavigationViewModel : BaseViewModel
    {
        protected readonly INavigator Navigator;

        private Source _source;

        private ICommand _navigateBackwardsCommand;

        private ICommand _navigateForwardsCommand;

        private ICommand _navigateToCommand;
        
        private ICommand _navigateToMetaDataCommand;

        public NavigationViewModel(INavigator navigator)
        {
            Navigator = navigator;
        }

        public Source Source
        {
            get => _source;
            set
            {
                if (_source == value) return;

                _source = value;
                OnPropertyChanged("Source");
            }
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
                return _navigateBackwardsCommand ?? (_navigateBackwardsCommand = new RelayCommand(
                    o => Navigator.NavigateBackward(),
                    o => Navigator.CanGoBackwards()
                ));
            }

            set => _navigateBackwardsCommand = value;
        }

        public ICommand NavigateForwardsCommand
        {
            get
            {
                return _navigateForwardsCommand ?? (_navigateForwardsCommand = new RelayCommand(
                    o => Navigator.NavigateForwards(),
                    o => Navigator.CanGoForwards()
                ));
            }
        }

        public ICommand NavigateToMetaDataCommand
        {
            get
            {
                return _navigateToMetaDataCommand ?? (_navigateToMetaDataCommand = new RelayCommand(
                    o => Navigator.NavigateTo(TargetView.MetaData, Source),
                    o => Source != null
                ));
            }
        }
    }
}