using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DotDll.Presentation.Navigation;

namespace DotDll.Presentation
{
    public partial class DotDllApp : Application
    {
        public INavigator Navigator;
    }

    public static class ApplicationExtensions
    {
        public static DotDllApp AsDotDllApp(this Application app)
        {
            return (DotDllApp) app;
        }
    }
}
