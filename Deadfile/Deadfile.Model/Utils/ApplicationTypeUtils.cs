using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;

namespace Deadfile.Model.Utils
{
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
