﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Tab.DesignTime;

namespace Deadfile.Tab.Home
{
    class HomePageDesignTimeViewModel : PageDesignTimeViewModel, IHomePageViewModel
    {
        public void AddClient()
        {
            throw new NotImplementedException();
        }

        public void LocalAuthorities()
        {
            throw new NotImplementedException();
        }

        public void DefineQuotations()
        {
            throw new NotImplementedException();
        }

        public void UnbilledJobs()
        {
            throw new NotImplementedException();
        }

        public void TodoReport()
        {
            throw new NotImplementedException();
        }

        public void ImportExportToJson()
        {
            throw new NotImplementedException();
        }

        public void UnpaidInvoices()
        {
            throw new NotImplementedException();
        }

        public void CurrentApplications()
        {
            throw new NotImplementedException();
        }

        public override Experience Experience
        {
            get { return Experience.Home; }
        }

        public override bool ShowActionsPad { get; } = true;
    }
}
