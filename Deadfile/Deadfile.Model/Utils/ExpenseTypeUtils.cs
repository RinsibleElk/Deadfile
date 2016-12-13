using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;

namespace Deadfile.Model.Utils
{
    /// <summary>
    /// Utilities for converting between <see cref="ExpenseType"/> and string.
    /// </summary>
    public static class ExpenseTypeUtils
    {
        /// <summary>
        /// Get the name of an <see cref="ExpenseType"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetName(ExpenseType type)
        {
            switch (type)
            {
                case ExpenseType.ApplicationFees:
                    return "Application Fees";
                case ExpenseType.ProfessionalFees:
                    return "Professional Fees";
                case ExpenseType.TitleDeedsOrPlans:
                    return "Title Deeds/Plans";
                case ExpenseType.Promap:
                    return "Promap";
                case ExpenseType.DrainageSearch:
                    return "Drainage Search";
                default:
                    return "Other";
            }
        }

        /// <summary>
        /// Get the <see cref="ExpenseType"/> from the name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ExpenseType GetType(string name)
        {
            switch (name)
            {
                case "Application Fees":
                    return ExpenseType.ApplicationFees;
                case "Professional Fees":
                    return ExpenseType.ProfessionalFees;
                case "Title Deeds/Plans":
                    return ExpenseType.TitleDeedsOrPlans;
                case "Promap":
                    return ExpenseType.Promap;
                case "Drainage Search":
                    return ExpenseType.DrainageSearch;
                default:
                    return ExpenseType.Other;
            }
        }
    }
}
