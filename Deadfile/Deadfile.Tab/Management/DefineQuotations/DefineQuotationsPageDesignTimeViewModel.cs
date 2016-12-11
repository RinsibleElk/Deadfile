using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Model;
using Deadfile.Model.Browser;
using Deadfile.Model.DesignTime;
using Deadfile.Tab.DesignTime;

namespace Deadfile.Tab.Management.DefineQuotations
{
    class DefineQuotationsPageDesignTimeViewModel :
        ManagementPageDesignTimeViewModel<QuotationModel>,
        IDefineQuotationsPageViewModel
    {
        public DefineQuotationsPageDesignTimeViewModel()
        {
            var repository = new DeadfileDesignTimeRepository();
            SelectedItem = new QuotationModel();
            Items = new ObservableCollection<QuotationModel>(repository.GetQuotations(null));
            Items.Add(SelectedItem);
        }

        // Stuff that every page has.
        public override Experience Experience { get; } = Experience.DefineQuotations;
    }
}
