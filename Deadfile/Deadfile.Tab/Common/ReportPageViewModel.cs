using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Model;
using MahApps.Metro.Controls.Dialogs;
using Prism.Events;

namespace Deadfile.Tab.Common
{
    abstract class ReportPageViewModel<T> : ManagementPageViewModel<T>, IReportPageViewModel<T> where T : ModelBase, new()
    {
        private readonly IPrintService _printService;
        protected ReportPageViewModel(IPrintService printService,
            IDialogCoordinator dialogCoordinator,
            IEventAggregator eventAggregator,
            bool allowAdds)
            : base(dialogCoordinator, eventAggregator, allowAdds)
        {
            _printService = printService;
        }

        protected abstract DataGrid GetVisual(object view);

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);

            _visual = GetVisual(view);
        }

        private DateTime _startDate = DateTime.Today;
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (value.Equals(_startDate)) return;
                _startDate = value;
                NotifyOfPropertyChange(() => StartDate);

                RefreshModels();
            }
        }

        private DateTime _endDate = DateTime.Today.AddDays(7.0);
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (value.Equals(_endDate)) return;
                _endDate = value;
                NotifyOfPropertyChange(() => EndDate);

                RefreshModels();
            }
        }

        private bool _includeInactive;
        private DataGrid _visual;

        public bool IncludeInactive
        {
            get { return _includeInactive; }
            set
            {
                if (value == _includeInactive) return;
                _includeInactive = value;
                NotifyOfPropertyChange(() => IncludeInactive);

                RefreshModels();
            }
        }

        public void Print()
        {
            _printService.PrintVisual(_visual);
        }

        protected sealed override void PerformDelete()
        {
            throw new NotImplementedException();
        }
        protected sealed override void PerformSave()
        {
            throw new NotImplementedException();
        }
        public sealed override void EditingStatusChanged(bool editable)
        {
            throw new NotImplementedException();
        }
    }
}
