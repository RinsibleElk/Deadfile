using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;

namespace Deadfile.Model
{
    public class ApplicationModel : JobChildModelBase
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
            set { SetProperty(ref _creationDate, value); }
        }

        private double _netAmount;
        [Required(ErrorMessage = "An Application must have an amount associated")]
        public double NetAmount
        {
            get { return _netAmount; }
            set { SetProperty(ref _netAmount, value); }
        }

        private ApplicationType _applicationType;
        [Required(ErrorMessage = "A Planning Application must have a type.")]
        public ApplicationType Type
        {
            get { return _applicationType; }
            set { SetProperty(ref _applicationType, value); }
        }
    }

    /// <summary>
    /// All the application types.
    /// </summary>
    public static class ApplicationTypeUtils
    {
        /// <summary>
        /// All the known application types.
        /// </summary>
        public static readonly List<ApplicationType> AllApplicationTypes = Enum.GetValues(typeof(ApplicationType)).Cast<ApplicationType>().ToList();

        public static string ConvertToString(ApplicationType applicationType)
        {
            switch (applicationType)
            {
                case ApplicationType.HouseholderPlanningPermission:
                    return "Householder Planning Permission";
                case ApplicationType.FullPlanningPermission:
                    return "Full Planning Permission";
                case ApplicationType.LawfulDevelopmentCertificate:
                    return "Lawful Development Certificate";
                case ApplicationType.MinorAmendment:
                    return "Minor Amendment";
                case ApplicationType.Appeal:
                    return "Appeal";
                case ApplicationType.ConservationAreaConsent:
                    return "Conservation Area Consent";
                case ApplicationType.ListedBuildingConsent:
                    return "Listed Building Consent";
                case ApplicationType.BuildingControlBuildingNotice:
                    return "Building Control - Building Notice";
                case ApplicationType.BuildingControlFullPlans:
                    return "Building Control - Full Plans";
                case ApplicationType.OutlinePermission:
                    return "Outline- Permission";
                case ApplicationType.PriorNotification:
                    return "Prior Notification";
                case ApplicationType.ConditionsApplication:
                    return "Conditions Application";
                case ApplicationType.DisplayOfAdvertisment:
                    return "Display of Advertisment";
                default:
                    throw new ApplicationException("Unknown application type: " + applicationType);
            }
        }
    }
}
