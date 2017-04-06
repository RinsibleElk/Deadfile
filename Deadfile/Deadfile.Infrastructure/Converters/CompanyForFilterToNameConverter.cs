using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Deadfile.Entity;

namespace Deadfile.Infrastructure.Converters
{
    /// <summary>
    /// Convert an object of type <see cref="CompanyForFilter"/> to its string name.
    /// </summary>
    [ValueConversion(typeof(CompanyForFilter), typeof(string))]
    public class CompanyForFilterToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string))
                throw new ApplicationException(
                    "Attempted to convert a company to something other than a string");
            var company = (CompanyForFilter) value;
            switch (company)
            {
                case CompanyForFilter.All:
                    return "All";
                case CompanyForFilter.PaulSamsonCharteredSurveyorLtd:
                    return "Paul Samson Chartered Surveyor Ltd";
                default:
                    return "Imagine 3D Ltd";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var companyName = (string) value;
            switch (companyName)
            {
                case "All":
                    return CompanyForFilter.All;
                case "Paul Samson Chartered Surveyor Ltd":
                    return CompanyForFilter.PaulSamsonCharteredSurveyorLtd;
                default:
                    return CompanyForFilter.Imagine3DLtd;
            }
        }
    }
}
