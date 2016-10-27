using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using Prism.Mvvm;

namespace Deadfile.Model.Browser
{
    public class BrowserSettings : BindableBase
    {
        private static readonly ObservableCollection<BrowserSort> ClientSorts = new ObservableCollection<BrowserSort>(new[] { BrowserSort.ClientFirstName, BrowserSort.ClientLastName });
        private static readonly ObservableCollection<BrowserSort> JobSorts = new ObservableCollection<BrowserSort>(new[] { BrowserSort.JobAddress });
        private static readonly ObservableCollection<BrowserSort> InvoiceSorts = new ObservableCollection<BrowserSort>(new[] {BrowserSort.InvoiceReference});
        private static readonly EventArgs EventArgs = new EventArgs();
        private bool _disableEvent = false;

        public BrowserSettings()
        {
            var modes = new[] {BrowserMode.Client, BrowserMode.Job, BrowserMode.Invoice};
            _modes = new ObservableCollection<BrowserMode>(modes);
            _mode = BrowserMode.Client;
            _sorts = ClientSorts;
            _sort = BrowserSort.ClientFirstName;
        }

        private string _filterText = "";

        public string FilterText
        {
            get { return _filterText; }
            set
            {
                if (SetProperty(ref _filterText, value) && !_disableEvent)
                    Refresh?.Invoke(this, EventArgs);
            }
        }

        private bool _includeInactiveEnabled = false;
        /// <summary>
        /// Whether the user wants to include inactive elements in their browser. False by default.
        /// </summary>
        public bool IncludeInactiveEnabled
        {
            get { return _includeInactiveEnabled; }
            set
            {
                if (SetProperty(ref _includeInactiveEnabled, value) && !_disableEvent)
                    Refresh?.Invoke(this, EventArgs);
            }
        }

        private ObservableCollection<BrowserMode> _modes;
        public ObservableCollection<BrowserMode> Modes
        {
            get { return _modes; }
            set { SetProperty(ref _modes, value); }
        }

        private BrowserMode _mode;
        public BrowserMode Mode
        {
            get { return _mode; }
            set
            {
                if (SetProperty(ref _mode, value) && !_disableEvent)
                {
                    _disableEvent = true;
                    switch (_mode)
                    {
                        case BrowserMode.Client:
                            Sorts = ClientSorts;
                            Sort = BrowserSort.ClientFirstName;
                            break;
                        case BrowserMode.Job:
                            Sorts = JobSorts;
                            Sort = BrowserSort.JobAddress;
                            break;
                        case BrowserMode.Invoice:
                            Sorts = InvoiceSorts;
                            Sort = BrowserSort.InvoiceReference;
                            break;
                    }
                    Refresh?.Invoke(this, EventArgs);
                    _disableEvent = false;
                }
            }
        }

        private ObservableCollection<BrowserSort> _sorts;
        public ObservableCollection<BrowserSort> Sorts
        {
            get { return _sorts; }
            set { SetProperty(ref _sorts, value); }
        }

        private BrowserSort _sort;
        public BrowserSort Sort
        {
            get { return _sort; }
            set
            {
                if (SetProperty(ref _sort, value) && !_disableEvent)
                    Refresh?.Invoke(this, EventArgs);
            }
        }

        public event EventHandler Refresh;
    }
}
