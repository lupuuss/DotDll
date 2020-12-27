using System.Windows;
using DotDll.Logic.Metadata;
using DotDll.Presentation.Navigation;

namespace DotDll.Presentation
{
    public partial class DotDllApp : Application
    {
        internal INavigator Navigator { get; set; }

        internal IMetadataService MetadataService { get; private set; }

        internal IUserInputService UserInputService { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MetadataService = new TempMetadataService();
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