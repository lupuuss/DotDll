using System;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace DotDll.Presentation.Navigation
{
    public interface IUserInputService
    { 
        Task<String> PickFilePath();
    }
}