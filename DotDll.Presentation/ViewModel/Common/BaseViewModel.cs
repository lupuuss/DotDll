using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DotDll.Presentation.ViewModel.Common
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

        protected void OnPropertyChangedAuto([CallerMemberName] String propertyName = "")
        {
            OnPropertyChanged(propertyName);
        }
    }
}