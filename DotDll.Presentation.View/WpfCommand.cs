using System;
using System.Windows.Input;
using DotDll.Presentation.ViewModel.Common;

namespace DotDll.Presentation.View
{
    public class WpfCommand : RelayCommand
    {
        public WpfCommand(Action<object> action, Predicate<object>? predicate = null) : base(action, predicate)
        {
        }

        public override event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public override void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}