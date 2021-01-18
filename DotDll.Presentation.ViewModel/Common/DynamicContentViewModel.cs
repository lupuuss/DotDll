using DotDll.Presentation.Model.Navigation;

namespace DotDll.Presentation.ViewModel.Common
{
    public class DynamicContentViewModel : NavigationViewModel
    {
        private bool _errorOccured;

        private bool _isContentShown;

        private bool _isLoading;

        public DynamicContentViewModel(INavigator navigator, RelayCommandFactory commandFactory) : base(navigator, commandFactory)
        {
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading == value) return;

                _isLoading = value;
                OnPropertyChangedAuto();
            }
        }

        public bool IsContentShown
        {
            get => _isContentShown;
            set
            {
                if (_isContentShown == value) return;

                _isContentShown = value;
                OnPropertyChangedAuto();
            }
        }

        public bool ErrorOccured
        {
            get => _errorOccured;
            set
            {
                if (_errorOccured == value) return;

                _errorOccured = value;
                OnPropertyChangedAuto();
            }
        }
    }
}