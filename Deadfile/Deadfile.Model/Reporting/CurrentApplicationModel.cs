﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;

namespace Deadfile.Model.Reporting
{
    /// <summary>
    /// Representation of an outstanding planning application - for reporting purposes.
    /// </summary>
    public class CurrentApplicationModel : ModelBase
    {
        // No need to report changes or validate.
        public override int Id
        {
            get { return ApplicationId; }
            set { ApplicationId = value; }
        }
        
        private int _applicationId;
        public int ApplicationId
        {
            get { return _applicationId; }
            set { SetProperty(ref _applicationId, value); }
        }

        private int _clientId;
        public int ClientId
        {
            get { return _clientId; }
            set { SetProperty(ref _clientId, value); }
        }

        private int _jobId;
        public int JobId
        {
            get { return _jobId; }
            set { SetProperty(ref _jobId, value); }
        }

        private string _jobAddressFirstLine;
        public string JobAddressFirstLine
        {
            get { return _jobAddressFirstLine; }
            set { SetProperty(ref _jobAddressFirstLine, value); }
        }

        private string _localAuthority;

        [Required(ErrorMessage = "An Application must have a local authority.")]
        public string LocalAuthority
        {
            get { return _localAuthority; }
            set { SetProperty(ref _localAuthority, value); }
        }

        private string _localAuthorityReference;

        [Required(ErrorMessage = "An Application must have a local authority reference.")]
        public string LocalAuthorityReference
        {
            get { return _localAuthorityReference; }
            set { SetProperty(ref _localAuthorityReference, value); }
        }

        private DateTime _creationDate = DateTime.Today;

        [Required(ErrorMessage = "A Planning Application must have a creation date.")]
        public DateTime CreationDate
        {
            get { return _creationDate; }
            set
            {
                if (SetProperty(ref _creationDate, value))
                {
                    DisableUndoTracking = true;
                    EstimatedDecisionDate = value.AddDays(8*7);
                    DisableUndoTracking = false;
                }
            }
        }

        private ApplicationType _applicationType;

        [Required(ErrorMessage = "A Planning Application must have a type.")]
        public ApplicationType Type
        {
            get { return _applicationType; }
            set { SetProperty(ref _applicationType, value); }
        }

        private DateTime _estimatedDecisionDate = DateTime.Today.AddDays(8*7);

        [Required(ErrorMessage = "An Application must have an estimated decision date.")]
        public DateTime EstimatedDecisionDate
        {
            get { return _estimatedDecisionDate; }
            set { SetProperty(ref _estimatedDecisionDate, value); }
        }

        private ApplicationState _state;
        [Required(ErrorMessage = "An Application must have a state.")]
        public ApplicationState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }
    }
}
