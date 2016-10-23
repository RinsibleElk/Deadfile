using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Deadfile.Content.Interfaces;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Prism.Mvvm;

namespace Deadfile.Content.Quotes
{
    public sealed class QuotesBarViewModel : BindableBase, IQuotesBarViewModel
    {
        private readonly IDeadfileRepository _repository;
        public QuotesBarViewModel(IDeadfileRepository repository)
        {
            _repository = repository;
            SetNewRandomQuotation();
            var timer =
                new DispatcherTimer(
                    TimeSpan.FromSeconds(10),
                    DispatcherPriority.ApplicationIdle,
                    SetNewRandomQuotationHandler,
                    Dispatcher.CurrentDispatcher);
            timer.Start();
        }

        private void SetNewRandomQuotation()
        {
            Quotation = _repository.GetRandomQuotation();
        }

        private void SetNewRandomQuotationHandler(object sender, EventArgs e)
        {
            SetNewRandomQuotation();
        }

        private QuotationModel _quotation;
        public QuotationModel Quotation
        {
            get { return _quotation; }
            set { SetProperty(ref _quotation, value); }
        }
    }
}
