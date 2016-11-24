using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Entity
{
    /// <summary>
    /// The set of companies that can be invoiced for.
    /// </summary>
    public enum Company
    {
        PaulSamsonCharteredSurveyorLtd,
        Imagine3DLtd
    }

    /// <summary>
    /// Utilities for displaying and manipulating <see cref="Company"/> values.
    /// </summary>
    public static class CompanyUtils
    {
        /// <summary>
        /// All the known companies.
        /// </summary>
        public static Company[] AllCompanies
        {
            get { return new Company[] {Company.PaulSamsonCharteredSurveyorLtd, Company.Imagine3DLtd}; }
        }

        /// <summary>
        /// Get the short name of the company for display purposes.
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetShortName(Company company)
        {
            switch (company)
            {
                case Company.PaulSamsonCharteredSurveyorLtd:
                    return "PKS";
                default:
                    return "I3D";
            }
        }

        /// <summary>
        /// Get the name of the company for display purposes.
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetName(Company company)
        {
            switch (company)
            {
                case Company.PaulSamsonCharteredSurveyorLtd:
                    return "Paul Samson Chartered Surveyor Ltd";
                default:
                    return "Imagine 3D Ltd";
            }
        }
    }
}
