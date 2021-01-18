using System;
using DotDll.Presentation.ViewModel.Common;

namespace DotDll.Presentation.View
{
    public class WpfRelayCommandFactory : RelayCommandFactory
    {
        public RelayCommand CreateCommand(Action<object> action, Predicate<object>? predicate = null)
        {
            return new WpfCommand(action, predicate);
        }
    }
}