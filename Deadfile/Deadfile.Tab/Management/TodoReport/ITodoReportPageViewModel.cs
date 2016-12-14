﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Model;
using Deadfile.Model.Reporting;
using Deadfile.Tab.Common;

namespace Deadfile.Tab.Management.TodoReport
{
    interface ITodoReportPageViewModel :
        IPageViewModel,
        IManagementViewModel<JobTaskModel>
    {
        ICommand NavigateToJob { get; }
        DateTime EndDate { get; set; }
    }
}