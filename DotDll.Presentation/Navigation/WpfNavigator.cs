using System;
using System.Windows.Controls;
using DotDll.Presentation.View;

namespace DotDll.Presentation.Navigation
{
    public class WpfNavigator : INavigator
    {
        private readonly Frame _frame;

        public WpfNavigator(Frame frame)
        {
            _frame = frame;
        }

        public void NavigateTo(TargetView target, params object[] args)
        {
            switch (target)
            {
                case TargetView.Menu:
                    _frame.Navigate(new MenuPage());
                    break;
                case TargetView.DeserializeList:
                    _frame.Navigate(new DeserializeListPage());
                    break;
                case TargetView.MetaData:
                    _frame.Navigate(new MetaDataPage());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(target), target, null);
            }
        }

        public void NavigateBackward()
        {
            _frame.GoBack();
        }

        public void NavigateForwards()
        {
            _frame.GoForward();
        }

        public bool CanGoBackwards()
        {
            return _frame.CanGoBack;
        }

        public bool CanGoForwards()
        {
            return _frame.CanGoForward;
        }
    }
}