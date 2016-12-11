using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Model;
using Deadfile.Tab.Common;

namespace Deadfile.Tab.Management.DefineQuotations
{
    interface IDefineQuotationsPageViewModel :
        IPageViewModel,
        IManagementViewModel<QuotationModel>
    {
        ICommand NavigateCommand { get; }
    }
}
