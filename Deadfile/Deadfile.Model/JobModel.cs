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
    /// UI model for a Job.
    /// </summary>
    public class JobModel : ModelBase
    {
        public override int Id
        {
            get { return JobId; }
            set { JobId = value; }
        }

        private int _jobId = ModelBase.NewModelId;
        /// <summary>
        /// This is Entity's model for this Job, or if it is not in the database yet then default.
        /// </summary>
        public int JobId
        {
            get { return _jobId; }
            set { SetProperty(ref _jobId, value); }
        }

        private int _jobNumber = 1;
        [Required(ErrorMessage = "You must provide a job number.")]
        public int JobNumber
        {
            get { return _jobNumber; }
            set { SetProperty(ref _jobNumber, value); }
        }

        private string _addressFirstLine;
        [Required(ErrorMessage = "You must provide an address for this Job."),
         MaxLength(200, ErrorMessage = "A line of an address must be less than 200 characters long.")]
        public string AddressFirstLine
        {
            get { return _addressFirstLine; }
            set { SetProperty(ref _addressFirstLine, value); }
        }

        private string _addressSecondLine;
        [MaxLength(200, ErrorMessage = "A line of an address must be less than 200 characters long.")]
        public string AddressSecondLine
        {
            get { return _addressSecondLine;}
            set { SetProperty(ref _addressSecondLine, value); }
        }

        private string _addressThirdLine;
        [MaxLength(200, ErrorMessage = "A line of an address must be less than 200 characters long.")]
        public string AddressThirdLine
        {
            get { return _addressThirdLine; }
            set { SetProperty(ref _addressThirdLine, value); }
        }

        private string _addressPostCode;
        [MaxLength(20, ErrorMessage = "A postcode must be less than 20 characters long.")]
        public string AddressPostCode
        {
            get { return _addressPostCode; }
            set { SetProperty(ref _addressPostCode, value); }
        }

        private JobStatus _status;
        public JobStatus Status
        {
            get { return _status;}
            set { SetProperty(ref _status, value); }
        }

        private string _notes;
        [MaxLength(500, ErrorMessage = "The free notes for a Job must be less than 500 characters long.")]
        public string Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value); }
        }

        private string _description;
        [Required(ErrorMessage = "Every Job must have a description"),
         MinLength(5, ErrorMessage = "The Description must have at least 5 characters"),
         MaxLength(50, ErrorMessage = "The Description must have at most 50 characters")]
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private int _clientId;
        public int ClientId
        {
            get { return _clientId; }
            set { SetProperty(ref _clientId, value); }
        }

    }
}
