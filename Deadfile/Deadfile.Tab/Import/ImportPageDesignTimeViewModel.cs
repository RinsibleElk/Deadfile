using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void BrowseJobs()
        {
            throw new NotImplementedException();
        }

        public string QuotationsFile { get; set; }

        public void BrowseQuotations()
        {
            throw new NotImplementedException();
        }

        public string LocalAuthoritiesFile { get; set; }

        public void BrowseLocalAuthorities()
        {
            throw new NotImplementedException();
        }
    }
}
