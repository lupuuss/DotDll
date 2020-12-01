using System.Windows;
using DotDll.Logic.MetaData;
using DotDll.Presentation.Navigation;

namespace DotDll.Presentation
{
    public partial class DotDllApp : Application
    {
        internal INavigator Navigator { get; set; }
        
        internal IMetaDataService MetaDataService { get; set; }
        
        internal IUserInputService UserInputService { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MetaDataService = new TempMetaDataService();
            UserInputService = new WpfUserInputService();
        }
    }

    public static class ApplicationExtensions
    {
        public static DotDllApp AsDotDllApp(this Application app)
        {
            return (DotDllApp) app;
        }
    }
}