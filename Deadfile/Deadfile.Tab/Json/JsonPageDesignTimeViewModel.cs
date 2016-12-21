using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Tab.DesignTime;

namespace Deadfile.Tab.Json
{
    class JsonPageDesignTimeViewModel : PageDesignTimeViewModel, IJsonPageViewModel
    {
        public void Import()
        {
            throw new NotImplementedException();
        }

        public bool CanImport { get; } = true;
        public string JsonFile { get; set; }
        public ICommand BrowseJson { get; } = null;
        public void Export()
        {
            throw new NotImplementedException();
        }
    }
}
