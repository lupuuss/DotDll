﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DotDll.Presentation.ViewModel.Common
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected RelayCommandFactory CommandFactory;

        public BaseViewModel(RelayCommandFactory commandFactory)
        {
            CommandFactory = commandFactory;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var e = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, e);
        }

        protected void OnPropertyChangedAuto([CallerMemberName] string propertyName = "")
        {
            OnPropertyChanged(propertyName);
        }
    }
}