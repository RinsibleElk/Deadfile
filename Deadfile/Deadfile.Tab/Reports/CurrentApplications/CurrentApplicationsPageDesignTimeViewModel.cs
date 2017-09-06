using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Deadfile.Entity;
using Deadfile.Model;
using Deadfile.Model.Browser;
using Deadfile.Model.DesignTime;
using Deadfile.Model.Reporting;
using Deadfile.Tab.DesignTime;

namespace Deadfile.Tab.Reports.CurrentApplications
{
    class CurrentApplicationsPageDesignTimeViewModel :
        ReportPageDesignTimeViewModel<CurrentApplicationModel>,
        ICurrentApplicationsPageViewModel
    {
        public CurrentApplicationsPageDesignTimeViewModel()
        {
            var repository = new DeadfileDesignTimeRepository();
            SelectedItem = null;
            var items = new List<CurrentApplicationModel>();
            items.Add(new CurrentApplicationModel
            {
                ApplicationId = 0,
                ClientId = 0,
                JobId = 0,
                Type = ApplicationType.Appeal,
                CreationDate = DateTime.Today.AddDays(-20),
                EstimatedDecisionDate = DateTime.Today,
                JobAddressFirstLine = "1 My Road",
                LocalAuthority = "Enfield",
                LocalAuthorityReference = "ENF0001",
                State = ApplicationState.Current
            });
            items.Add(new CurrentApplicationModel
            {
                ApplicationId = 1,
                ClientId = 0,
                JobId = 0,
                Type = ApplicationType.ConservationAreaConsent,
                CreationDate = DateTime.Today.AddDays(-10),
                EstimatedDecisionDate = DateTime.Today.AddDays(10),
                JobAddressFirstLine = "2 My Road",
                LocalAuthority = "Enfield",
                LocalAuthorityReference = "ENF0002",
                State = ApplicationState.Current
            });
            items.Add(new CurrentApplicationModel
            {
                ApplicationId = 2,
                ClientId = 1,
                JobId = 1,
                Type = ApplicationType.BuildingControlFullPlans,
                CreationDate = DateTime.Today.AddDays(-5),
                EstimatedDecisionDate = DateTime.Today.AddDays(15),
                JobAddressFirstLine = "3 Your Street",
                LocalAuthority = "Barnet",
                LocalAuthorityReference = "BAR0003",
                State = ApplicationState.Withdrawn
            });
            items.Add(new CurrentApplicationModel
            {
                ApplicationId = 3,
                ClientId = 2,
                JobId = 2,
                Type = ApplicationType.FullPlanningPermission,
                CreationDate = DateTime.Today.AddDays(-2),
                EstimatedDecisionDate = DateTime.Today.AddDays(21),
                JobAddressFirstLine = "4 His Mews",
                LocalAuthority = "Brent",
                LocalAuthorityReference = "BRE0004",
                State = ApplicationState.Withdrawn
            });
            Items = new ObservableCollection<CurrentApplicationModel>(items);
        }

        // Stuff that every page has.
        public override Experience Experience { get; } = Experience.CurrentApplications;

        public ICommand NavigateToJob { get; } = null;
        public ICommand NavigateToJobAndEdit { get; } = null;
        public DataGrid DataGrid { get; } = null;
    }
}
