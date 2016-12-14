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
    [ValueConversion(typeof(JobTaskState), typeof(bool?))]
    public class JobTaskToNullableBooleanStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool?))
                throw new InvalidOperationException("The target must be a bool?");
            var state = (JobTaskState)value;
            return state == JobTaskState.Completed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var completed = value as bool?;
            if (completed == null) return JobTaskState.Active;
            if (!completed.Value) return JobTaskState.Active;
            return JobTaskState.Completed;
        }
    }
}
