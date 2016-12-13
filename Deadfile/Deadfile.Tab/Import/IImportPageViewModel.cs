using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Tab.Common;

namespace Deadfile.Tab.Import
{
    interface IImportPageViewModel : IPageViewModel
    {
        void Import();
        bool CanImport { get; }
        string JobsFile { get; set; }
        ICommand BrowseJobs { get; }
        string QuotationsFile { get; set; }
        ICommand BrowseQuotations { get; }
        string LocalAuthoritiesFile { get; set; }
        ICommand BrowseLocalAuthorities { get; }
    }
}
