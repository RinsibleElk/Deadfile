using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Deadfile.Entity;
using Deadfile.Model.Utils;

namespace Deadfile.Infrastructure.Converters
{
    [ValueConversion(typeof(ExpenseType), typeof(string))]
    public class ExpenseTypeToStringConverter : IValueConverter
    {
        /// <summary>
        /// Convert an <see cref="ExpenseType"/> to a string for display.
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
                    "Attempted to convert an expense type to something other than a string");
            var expenseType = (ExpenseType)value;
            return ExpenseTypeUtils.GetName(expenseType);

        }

        /// <summary>
        /// Convert a string representation of an <see cref="ExpenseType"/> to a strongly typed identifier.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var name = (string) value;
            return ExpenseTypeUtils.GetType(name);
        }
    }
}
