using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;

namespace Deadfile.Model
{
    /// <summary>
    /// UI model for an Invoice.
    /// </summary>
    public class InvoiceModel : ModelBase
    {
        public override int Id
        {
            get { return InvoiceId; }
            set { InvoiceId = value; }
        }

        private int _invoiceId = ModelBase.NewModelId;
        public int InvoiceId
        {
            get { return _invoiceId; }
            set { SetProperty(ref _invoiceId, value); }
        }

        private DateTime _createdDate;

        [Required(ErrorMessage = "An Invoice must have a creation date.")]
        public DateTime CreatedDate
        {
            get { return _createdDate; }
            set { SetProperty(ref _createdDate, value); }
        }

        private double _grossAmount = 0;
        public double GrossAmount
        {
            get { return _grossAmount; }
            set { SetProperty(ref _grossAmount, value); }
        }

        private double _netAmount = 0;
        public double NetAmount
        {
            get { return _netAmount; }
            set { SetProperty(ref _netAmount, value); }
        }

        private InvoiceStatus _status;
        [Required(ErrorMessage = "The Invoice requires a Status.")]
        public InvoiceStatus Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        private int _invoiceReference = ModelBase.NewModelId;
        public int InvoiceReference
        {
            get { return _invoiceReference; }
            set { SetProperty(ref _invoiceReference, value); }
        }

        private Company _company = Company.PaulSamsonCharteredSurveyorLtd;
        [Required(ErrorMessage = "An Invoice requires a Company that it has been issued for.")]
        public Company Company
        {
            get { return _company; }
            set { SetProperty(ref _company, value); }
        }
    }
}
