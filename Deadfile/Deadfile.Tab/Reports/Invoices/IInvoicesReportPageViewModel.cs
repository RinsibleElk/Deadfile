using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Model;
using Deadfile.Model.Reporting;
using Deadfile.Tab.Common;

namespace Deadfile.Tab.Reports.Invoices
{
    interface IInvoicesReportPageViewModel : IReportPageViewModel<InvoiceModel>, IDataGridPresenter
    {
        ICommand NavigateToClient { get; }
        ICommand NavigateToInvoice { get; }
        void ExportToExcel();
    }
}
