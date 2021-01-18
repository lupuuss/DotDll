using System.Windows;
using DotDll.Logic.Metadata;
using DotDll.Presentation.Model.Navigation;
using DotDll.Presentation.View.Navigation;
using DotDll.Presentation.ViewModel.Common;

namespace DotDll.Presentation.View
{
    public partial class DotDllApp
    {
        internal INavigator Navigator { get; set; } = null!;

        internal IMetadataService MetadataServiceImpl { get; private set; } = null!;

        internal IUserInputService UserInputService { get; private set; } = null!;
        
        internal RelayCommandFactory RelayCommandFactory { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MetadataServiceImpl = MetadataService.CreateDefault();
            UserInputService = new WpfUserInputService();
            RelayCommandFactory = new WpfRelayCommandFactory();
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