using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
using Deadfile.Model.Interfaces;

namespace Deadfile.Model.Browser
{
    public sealed class BrowserClient : BrowserModel
    {
        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set { SetProperty(ref _fullName, value); }
        }

        /// <summary>
        /// Only get called when in Client mode.
        /// </summary>
        protected override void LoadChildren()
        {
            if (Mode == BrowserMode.Client)
                foreach (var job in Repository.GetBrowserJobsForClient(Mode, IncludeInactiveEnabled, Id))
                    Children.Add(job);
        }

        public override BrowserModelType ModelType => BrowserModelType.Client;

        private ClientStatus _clientStatus = ClientStatus.Active;
        public ClientStatus Status
        {
            get { return _clientStatus; }
            set { SetProperty(ref _clientStatus, value); }
        }
    }
}
