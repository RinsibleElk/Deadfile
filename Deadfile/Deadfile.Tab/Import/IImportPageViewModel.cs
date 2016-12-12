using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Tab.Common;

namespace Deadfile.Tab.Import
{
    interface IImportPageViewModel : IPageViewModel
    {
        void Import();
        bool CanImport { get; }
        string JobsFile { get; set; }
        void BrowseJobs();
        string QuotationsFile { get; set; }
        void BrowseQuotations();
        string LocalAuthoritiesFile { get; set; }
        void BrowseLocalAuthorities();
    }
}
