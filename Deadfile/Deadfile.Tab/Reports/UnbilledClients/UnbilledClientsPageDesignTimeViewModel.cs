using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Model;
using Deadfile.Model.Browser;
using Deadfile.Model.DesignTime;
using Deadfile.Model.Reporting;
using Deadfile.Tab.DesignTime;

namespace Deadfile.Tab.Reports.UnbilledClients
{
    class UnbilledClientsPageDesignTimeViewModel :
        ReportPageDesignTimeViewModel<UnbilledClientModel>,
        IUnbilledClientsPageViewModel
    {
        public UnbilledClientsPageDesignTimeViewModel()
        {
            var repository = new DeadfileDesignTimeRepository();
            SelectedItem = null;
            Items = new ObservableCollection<UnbilledClientModel>();
        }

        // Stuff that every page has.
        public override Experience Experience { get; } = Experience.UnbilledClients;

        public ICommand NavigateToClient
        {
            get { throw new NotImplementedException(); }
        }
    }
}
