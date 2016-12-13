using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Tab.DesignTime;

namespace Deadfile.Tab.Import
{
    class ImportPageDesignTimeViewModel : PageDesignTimeViewModel, IImportPageViewModel
    {
        public void Import()
        {
            throw new NotImplementedException();
        }

        public bool CanImport { get; } = true;
        public string JobsFile { get; set; }

        public ICommand BrowseJobs { get; } = null;

        public string QuotationsFile { get; set; }

        public ICommand BrowseQuotations { get; } = null;

        public string LocalAuthoritiesFile { get; set; }

        public ICommand BrowseLocalAuthorities { get; } = null;
    }
}
