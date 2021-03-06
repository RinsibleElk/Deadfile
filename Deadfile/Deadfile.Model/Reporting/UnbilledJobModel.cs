﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model.Billable;

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
        /// <summary>
        /// The address of this unbilled job.
        /// </summary>
        public string AddressFirstLine
        {
            get { return _addressFirstLine; }
            set { SetProperty(ref _addressFirstLine, value); }
        }

        private int _jobNumber;
        /// <summary>
        /// The job number for this unbilled job.
        /// </summary>
        public int JobNumber
        {
            get { return _jobNumber; }
            set { SetProperty(ref _jobNumber, value); }
        }

        private string _fullName;
        /// <summary>
        /// The full name of the client for this unbilled job.
        /// </summary>
        public string FullName
        {
            get { return _fullName; }
            set { SetProperty(ref _fullName, value); }
        }

        private double _unbilledAmount;
        /// <summary>
        /// Net amount uninvoiced for this unbilled job.
        /// </summary>
        public double UnbilledAmount
        {
            get { return _unbilledAmount; }
            set { SetProperty(ref _unbilledAmount, value); }
        }

        /// <summary>
        /// Hours uninvoiced for this unbilled job.
        /// </summary>
        private double _unbilledHours;
        public double UnbilledHours
        {
            get { return _unbilledHours; }
            set { SetProperty(ref _unbilledHours, value); }
        }

        /// <summary>
        /// Items for this unbilled job.
        /// </summary>
        public ObservableCollection<UnbilledItemModel> Items { get; } = new ObservableCollection<UnbilledItemModel>();

        /// <summary>
        /// Header string to display to the user.
        /// </summary>
        public string HeaderString => $"{FullName} ({UnbilledAmount.ToString("C", CultureInfo.CurrentCulture)}, {UnbilledHours} hours)";
    }
}
