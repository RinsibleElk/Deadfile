using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Tab.DesignTime;

namespace Deadfile.Tab.Json
{
    class ImportPageDesignTimeViewModel : PageDesignTimeViewModel, IImportPageViewModel
    {
        public void Import()
        {
            throw new NotImplementedException();
        }

        public bool CanImport { get; } = true;
        public string JsonFile { get; set; }
        public ICommand BrowseJson { get; } = null;
    }
}
