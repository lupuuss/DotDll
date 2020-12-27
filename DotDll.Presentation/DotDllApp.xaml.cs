using System.Windows;
using DotDll.Logic.Metadata;
using DotDll.Presentation.Navigation;

namespace DotDll.Presentation
{
    public partial class DotDllApp
    {
        internal INavigator Navigator { get; set; } = null!;

        internal IMetadataService MetadataService { get; private set; } = null!;

        internal IUserInputService UserInputService { get; private set; } = null!;

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