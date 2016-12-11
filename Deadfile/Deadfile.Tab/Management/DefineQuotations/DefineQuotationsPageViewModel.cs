using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Common;
using Deadfile.Tab.Home;
using Prism.Commands;
using IEventAggregator = Prism.Events.IEventAggregator;

namespace Deadfile.Tab.Management.DefineQuotations
{
    /// <summary>
    /// View model for the Define Quotations Experience. Allows management of the known set of quotations.
    /// </summary>
    class DefineQuotationsPageViewModel : ManagementPageViewModel<QuotationModel>, IDefineQuotationsPageViewModel
    {
        private readonly IDeadfileRepository _repository;

        /// <summary>
        /// Create a new <see cref="DefineQuotationsPageViewModel"/>.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="eventAggregator"></param>
        public DefineQuotationsPageViewModel(IDeadfileRepository repository, IEventAggregator eventAggregator) : base(eventAggregator, true)
        {
            _repository = repository;
        }

        /// <summary>
        /// Perform the database interaction.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<QuotationModel> GetModels(string filter)
        {
            return _repository.GetQuotations(filter);
        }

        protected override void PerformSave()
        {
            _repository.SaveQuotation(SelectedItem);
        }

        // Common for every journaled page (content).
        public override Experience Experience { get; } = Experience.DefineQuotations;
        public override void EditingStatusChanged(bool editable)
        {
        }
    }
}
