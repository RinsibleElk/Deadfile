﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Deadfile.Model;
using Deadfile.Model.DesignTime;
using Deadfile.Tab.DesignTime;

namespace Deadfile.Tab.JobChildren.Applications
{
    class ApplicationsJobChildDesignTimeViewModel : ManagementPageDesignTimeViewModel<ApplicationModel>, IApplicationsJobChildViewModel
    {
        public ApplicationsJobChildDesignTimeViewModel()
        {
            SelectedItem = new ApplicationModel();
            Items = new ObservableCollection<ApplicationModel>();
            var repository = new DeadfileDesignTimeRepository();
            foreach (var application in repository.GetApplicationsForJob(4, null))
            {
                Items.Add(application);
            }
            Items.Add(SelectedItem);
            LocalAuthorities = new ObservableCollection<string>(repository.GetLocalAuthorities(null).Select((la) => la.Name));
        }

        public ObservableCollection<string> LocalAuthorities { get; set; }
    }
}
