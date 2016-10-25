using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace Deadfile.Model.Browser
{
    public class FilterSettings : BindableBase
    {
        private bool _includeInactiveEnabled = false;
        /// <summary>
        /// Whether the user wants to include inactive elements in their browser. False by default.
        /// </summary>
        public bool IncludeInactiveEnabled
        {
            get { return _includeInactiveEnabled; }
            set { SetProperty(ref _includeInactiveEnabled, value); }
        }

        private bool _filterByJobEnabled = false;
        /// <summary>
        /// Whether the user wants to filter by job address. False by default.
        /// </summary>
        public bool FilterByJobEnabled
        {
            get { return _filterByJobEnabled; }
            set { SetProperty(ref _filterByJobEnabled, value); }
        }

        private bool _filterByInvoiceEnabled = false;
        /// <summary>
        /// Whether the user wants to filter by invoice reference number. False by default.
        /// </summary>
        public bool FilterByInvoiceEnabled
        {
            get { return _filterByInvoiceEnabled; }
            set { SetProperty(ref _filterByInvoiceEnabled, value); }
        }
    }
}
