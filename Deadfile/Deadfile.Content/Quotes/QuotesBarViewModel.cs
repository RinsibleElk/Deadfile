using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Deadfile.Content.Interfaces;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Prism.Commands;
using Prism.Mvvm;

namespace Deadfile.Content.Quotes
{
    public sealed class QuotesBarViewModel : BindableBase, IQuotesBarViewModel
    {
        private readonly IDeadfileRepository _repository;
        public QuotesBarViewModel(IDeadfileRepository repository, IQuotationsTimerService quotationsTimerService)
        {
            _repository = repository;
            SetNewRandomQuotation();
            quotationsTimerService.StartTimer(SetNewRandomQuotationHandler);
            _nextQuotationCommand = new DelegateCommand(SetNewRandomQuotation);
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

        private readonly DelegateCommand _nextQuotationCommand;

        public ICommand NextQuotationCommand { get { return _nextQuotationCommand; } }
    }
}
