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
    [ValueConversion(typeof(int), typeof(string))]
    public class ModelIdToEditCommandNameConverter : IValueConverter
    {
        /// <summary>
        /// Convert a ModelBase.NewModelId to "Add" and everything else to "Edit".
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
                    "Attempted to convert a model id to something other than a string");
            if (value.GetType() != typeof(int)) return "Edit";
            var modelId = (int)value;
            return (modelId == ModelBase.NewModelId) ? "Add" : "Edit";
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
            throw new NotImplementedException();
        }
    }
}
