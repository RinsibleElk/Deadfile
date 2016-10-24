using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private IDeadfileRepository _repository;
        internal void SetRepository(IDeadfileRepository repository)
        {
            _repository = repository;
        }

        public BrowserClient() : base(false)
        {
            Id = ClientModel.NewClientId;
        }

        protected override void LoadChildren()
        {
            foreach (var job in _repository.GetBrowserJobsForClient(Id))
                Children.Add(job);
        }

        public override BrowserModelType ModelType
        {
            get { return BrowserModelType.Client; }
        }
    }
}
