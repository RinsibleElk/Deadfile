﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Tab.Common;

namespace Deadfile.Tab.Home
{
    interface IHomePageViewModel : IPageViewModel
    {
        void AddClient();
        void LocalAuthorities();
        void DefineQuotations();
        void UnbilledJobs();
        void TodoReport();
        void Import();
        void Export();
        void InvoicesReport();
        void CurrentApplications();
    }
}
