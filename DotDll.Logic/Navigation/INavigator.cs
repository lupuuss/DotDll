namespace DotDll.Logic.Navigation
{
    public enum TargetView
    {
        Menu,
        DeserializeList,
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