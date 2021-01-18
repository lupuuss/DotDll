using System;
using DotDll.Presentation.ViewModel.Common;

namespace DotDll.Tests.Presentation.ViewModel
{
    public class TestRelayCommand : RelayCommand
    {
        public TestRelayCommand(Action<object> action, Predicate<object>? predicate = null) : base(action, predicate)
        {
        }

        public override event EventHandler CanExecuteChanged;
        public override void RaiseCanExecuteChanged()
        {
        }
    }
}