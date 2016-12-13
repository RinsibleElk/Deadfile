using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Deadfile.Entity;
using Deadfile.Model;
using Deadfile.Model.Utils;

namespace Deadfile.Infrastructure.Converters
{
    [ValueConversion(typeof(ApplicationType), typeof(string))]
    public class ApplicationTypeToStringConverter : IValueConverter
    {
        /// <summary>
        /// Convert camel case to spaces and '_' to ' - ' and ' Of ' to ' of '.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string))
                throw new ApplicationException(
                    "Attempted to convert an application type to something other than a string");
            var applicationType = (ApplicationType) value;
            return ApplicationTypeUtils.ConvertToString(applicationType);
        }

        /// <summary>
        /// Convert back into an application type.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = (string) value;
            switch (stringValue)
            {
                case "Householder Planning Permission":
                    return ApplicationType.HouseholderPlanningPermission;
                case "Full Planning Permission":
                    return ApplicationType.FullPlanningPermission;
                case "Lawful Development Certificate":
                    return ApplicationType.LawfulDevelopmentCertificate;
                case "Minor Amendment":
                    return ApplicationType.MinorAmendment;
                case "Appeal":
                    return ApplicationType.Appeal;
                case "Conservation Area Consent":
                    return ApplicationType.ConservationAreaConsent;
                case "Listed Building Consent":
                    return ApplicationType.ListedBuildingConsent;
                case "Building Control - Building Notice":
                    return ApplicationType.BuildingControlBuildingNotice;
                case "Building Control - Full Plans":
                    return ApplicationType.BuildingControlFullPlans;
                case "Outline- Permission":
                    return ApplicationType.OutlinePermission;
                case "Prior Notification":
                    return ApplicationType.PriorNotification;
                case "Conditions Application":
                    return ApplicationType.ConditionsApplication;
                case "Display of Advertisment":
                    return ApplicationType.DisplayOfAdvertisment;
                default:
                    throw new ApplicationException("Unknown application type: " + stringValue);
            }
        }
    }
}
