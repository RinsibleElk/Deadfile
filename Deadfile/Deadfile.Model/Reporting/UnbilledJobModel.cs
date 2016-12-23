using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model.Reporting
{
    /// <summary>
    /// Representation of a client who has unbilled items attached.
    /// </summary>
    public class UnbilledJobModel : ModelBase
    {
        public override int Id
        {
            get { return ClientId; }
            set { ClientId = value; }
        }

        private int _clientId;
        /// <summary>
        /// EntityFramework's client id.
        /// </summary>
        public int ClientId
        {
            get { return _clientId; }
            set { SetProperty(ref _clientId, value); }
        }

        private int _jobId;
        /// <summary>
        /// EntityFramework's job id.
        /// </summary>
        public int JobId
        {
            get { return _jobId; }
            set { SetProperty(ref _jobId, value); }
        }

        private string _addressFirstLine;
        public string AddressFirstLine
        {
            get { return _addressFirstLine; }
            set { SetProperty(ref _addressFirstLine, value); }
        }

        private string _fullName;
        /// <summary>
        /// The full name of this client.
        /// </summary>
        public string FullName
        {
            get { return _fullName; }
            set { SetProperty(ref _fullName, value); }
        }

        private double _unbilledAmount;
        /// <summary>
        /// Net amount uninvoiced for this client.
        /// </summary>
        public double UnbilledAmount
        {
            get { return _unbilledAmount; }
            set { SetProperty(ref _unbilledAmount, value); }
        }

        public string HeaderString => $"{FullName} ({UnbilledAmount.ToString("C", CultureInfo.CurrentCulture)})";
    }
}
