using System;

namespace DotDll.Presentation.ViewModel.Common
{
    public interface RelayCommandFactory
    {
        RelayCommand CreateCommand(Action<object> action, Predicate<object>? predicate = null);
    }
}