﻿using System;
using System.Windows.Input;

namespace DotDll.Presentation.ViewModel.Common
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _action;
        private readonly Predicate<object>? _predicate;

        public RelayCommand(Action<object> action, Predicate<object>? predicate = null)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
            _predicate = predicate;
        }

        public bool CanExecute(object parameter)
        {
            return _predicate?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}