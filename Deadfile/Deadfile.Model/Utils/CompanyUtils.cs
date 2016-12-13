using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;

namespace Deadfile.Model.Utils
{
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
            get { return new Company[] { Company.Imagine3DLtd, Company.PaulSamsonCharteredSurveyorLtd }; }
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
                    return "Imagine3D Ltd";
            }
        }
    }
}
