using DotDll.Presentation.Navigation;

namespace DotDll.Presentation.ViewModel
{
    public class DynamicContentViewModel : NavigationViewModel
    {
        
        private bool _isLoading = false;

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading == value) return;

                _isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }

        private bool _isContentShown = false;

        public bool IsContentShown
        {
            get => _isContentShown;
            set
            {
                if (_isContentShown == value) return;

                _isContentShown = value;
                OnPropertyChanged("IsContentShown");
            }
        }
        
        private bool _errorOccured = false;

        public bool ErrorOccured
        {
            get => _errorOccured;
            set
            {
                if (_errorOccured == value) return;

                _errorOccured = value;
                OnPropertyChanged("ErrorOccured");
            }
        }

        public DynamicContentViewModel(INavigator navigator) : base(navigator)
        {
        }
    }
}