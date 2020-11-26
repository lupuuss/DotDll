namespace DotDll.Presentation.Navigation
{
    public enum TargetView
    {
        Menu,
        List,
        MetaData
    }

    public interface INavigator
    {
        void NavigateTo(TargetView target, params object[] args);
        void NavigateBackward();
        bool CanGoBackwards();
        void NavigateForwards();
        bool CanGoForwards();
    }
}