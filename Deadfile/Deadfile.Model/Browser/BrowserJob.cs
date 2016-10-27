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

        private IDeadfileRepository _repository;
        internal void SetRepository(IDeadfileRepository repository)
        {
            _repository = repository;
        }

        public BrowserJob() : base(false)
        {
            Id = ModelBase.NewModelId;
        }

        protected override void LoadChildren()
        {
            foreach (var invoice in _repository.GetBrowserInvoicesForJob(Id))
                Children.Add(invoice);
        }

        public override BrowserModelType ModelType
        {
            get { return BrowserModelType.Job; }
        }
    }
}
