using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Entity
{
    /// <summary>
    /// All the application types. Static at present.
    /// </summary>
    public enum ApplicationType
    {
        HouseholderPlanningPermission,
        FullPlanningPermission,
        LawfulDevelopmentCertificate,
        MinorAmendment,
        Appeal,
        ConservationAreaConsent,
        ListedBuildingConsent,
        BuildingControlBuildingNotice,
        BuildingControlFullPlans,
        OutlinePermission,
        PriorNotification,
        ConditionsApplication,
        DisplayOfAdvertisment
    }
}
