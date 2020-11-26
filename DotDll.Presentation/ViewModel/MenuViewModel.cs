using System.Windows.Input;
using DotDll.Presentation.Navigation;

namespace DotDll.Presentation.ViewModel
{
    public class MenuViewModel : NavigationViewModel
    {
        private string _pickedFilePath;

        public MenuViewModel(INavigator navigator) : base(navigator)
        {
        }

        public string PickedFilePath
        {
            get => _pickedFilePath;
            set
            {
                if (_pickedFilePath == value) return;

                _pickedFilePath = value;
                OnPropertyChanged("PickedFilePath");
            }
        }

        public ICommand PickFileCommand { get; set; }
    }
}