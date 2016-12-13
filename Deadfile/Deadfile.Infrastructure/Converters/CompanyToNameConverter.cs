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
    /// Convert an object of type <see cref="Company"/> to its string name.
    /// </summary>
    [ValueConversion(typeof(Company), typeof(string))]
    public class CompanyToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string))
                throw new ApplicationException(
                    "Attempted to convert a company to something other than a string");
            var company = (Company) value;
            switch (company)
            {
                case Company.PaulSamsonCharteredSurveyorLtd:
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
                case "Paul Samson Chartered Surveyor Ltd":
                    return Company.PaulSamsonCharteredSurveyorLtd;
                default:
                    return Company.Imagine3DLtd;
            }
        }
    }
}
