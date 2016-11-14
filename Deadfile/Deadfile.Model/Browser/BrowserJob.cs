using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model.Interfaces;

namespace Deadfile.Model.Browser
{
    public sealed class BrowserJob : BrowserModel
    {
        private string _fullAddress;
        public string FullAddress
        {
            get { return _fullAddress; }
            set { SetProperty(ref _fullAddress, value); }
        }

        protected override void LoadChildren()
        {
            if ((Mode == BrowserMode.Client) || (Mode == BrowserMode.Job))
                foreach (var invoice in Repository.GetBrowserInvoicesForJob(Mode, IncludeInactiveEnabled, Id))
                    Children.Add(invoice);
            if (Mode == BrowserMode.Job)
                Children.Add(Repository.GetBrowserClientById(Mode, IncludeInactiveEnabled, ParentId));
        }

        public override BrowserModelType ModelType
        {
            get { return BrowserModelType.Job; }
        }

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value); }
        }
    }
}
