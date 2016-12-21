using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Tab.Common;

namespace Deadfile.Tab.Json
{
    interface IJsonPageViewModel : IPageViewModel
    {
        void Import();
        bool CanImport { get; }
        string JsonFile { get; set; }
        ICommand BrowseJson { get; }
        void Export();
    }
}
