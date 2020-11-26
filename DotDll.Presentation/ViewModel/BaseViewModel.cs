using System.ComponentModel;

namespace DotDll.Presentation.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            
            if (handler == null) return;
            
            var e = new PropertyChangedEventArgs(propertyName); 
            handler(this, e);
        }
    }
}