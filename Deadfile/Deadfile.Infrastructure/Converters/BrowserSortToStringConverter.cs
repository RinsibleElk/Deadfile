using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Deadfile.Model.Browser;

namespace Deadfile.Infrastructure.Converters
{
    [ValueConversion(typeof(BrowserSort), typeof(string))]
    public class BrowserSortToStringConverter : IValueConverter
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
                    "Attempted to convert a browser sort to something other than a string");
            if (value == null) return "";
            if (value.GetType() != typeof(BrowserSort)) return "";
            var browserSort = (BrowserSort) value;
            switch (browserSort)
            {
                case BrowserSort.ClientFirstName:
                    return "Client First Name";
                case BrowserSort.ClientLastName:
                    return "Client Last Name";
                case BrowserSort.InvoiceReference:
                    return "Invoice Reference";
                case BrowserSort.JobAddress:
                    return "Job Address";
                case BrowserSort.InvoiceCreationDate:
                    return "Invoice Creation Date";
            }
            return browserSort.ToString();
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
                case "Client First Name":
                    return BrowserSort.ClientFirstName;
                case "Client Last Name":
                    return BrowserSort.ClientLastName;
                case "Invoice Reference":
                    return BrowserSort.InvoiceReference;
                case "Job Address":
                    return BrowserSort.JobAddress;
                case "Invoice Creation Date":
                    return BrowserSort.InvoiceCreationDate;
            }
            return (BrowserSort)Enum.Parse(targetType, stringValue);
        }
    }
}
