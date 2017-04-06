using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using Deadfile.Entity;
using Deadfile.Infrastructure;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Model;
using MahApps.Metro.Controls.Dialogs;
using Prism.Events;

namespace Deadfile.Tab.Common
{
    internal abstract class ReportPageViewModel<T> : ManagementPageViewModel<T>, IReportPageViewModel<T> where T : ModelBase, new()
    {
        private readonly IPrintService _printService;
        protected ReportPageViewModel(IPrintService printService,
            IDeadfileDialogCoordinator dialogCoordinator,
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

        private CompanyForFilter _companyFilter = CompanyForFilter.All;
        public CompanyForFilter CompanyFilter
        {
            get { return _companyFilter; }
            set
            {
                if (value.Equals(_companyFilter)) return;
                _companyFilter = value;
                NotifyOfPropertyChange(() => CompanyFilter);

                RefreshModels();
            }
        }

        private DateTime _startDate = DateTime.MinValue;
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

        private DateTime _endDate = DateTime.MaxValue;
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

        private DataGrid _visual;
        public Visual Visual => _visual;

        public void Print()
        {
            _printService.PrintVisual(this);
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
