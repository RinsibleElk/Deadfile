using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Deadfile.Model.Billable;

namespace Deadfile.Infrastructure.Converters
{
    public class BillableModelStateToIsEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new ApplicationException(
                    "Attempted to convert a billable model state to something other than a bool?");
            var state = (BillableModelState)value;
            switch (state)
            {
                case BillableModelState.Claimed:
                    return false;
                default:
                    return true;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
