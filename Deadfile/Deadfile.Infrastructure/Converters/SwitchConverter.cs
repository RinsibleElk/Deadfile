using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Deadfile.Infrastructure.Converters
{
    /// <summary>
    /// A converter that accepts <see cref="SwitchConverterCase"/>s and converts them to the 
    /// Then property of the case.
    /// </summary>
    [ContentProperty("Cases")]
    public class SwitchConverter : IValueConverter
    {
        // Converter instances.

        #region Public Properties.
        /// <summary>
        /// Gets or sets an array of <see cref="SwitchConverterCase"/>s that this converter can use to produde values from.
        /// </summary>
        public List<SwitchConverterCase> Cases { get; set; }

        #endregion
        #region Construction.
        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchConverter"/> class.
        /// </summary>
        public SwitchConverter()
        {
            // Create the cases array.
            Cases = new List<SwitchConverterCase>();
        }
        #endregion

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // This will be the results of the operation.

            // I'm only willing to convert SwitchConverterCases in this converter and no nulls!
            if (value == null) throw new ArgumentNullException(nameof(value));

            // I need to find out if the case that matches this value actually exists in this converters cases collection.
            if (Cases == null || Cases.Count <= 0) return null;

            // return the results.
            return (from targetCase in Cases where value == targetCase || value.ToString().ToUpper() == targetCase.When.ToString().ToUpper() select targetCase.Then).FirstOrDefault();
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Represents a case for a switch converter.
    /// </summary>
    [ContentProperty("Then")]
    public class SwitchConverterCase
    {
        // case instances.

        #region Public Properties.
        /// <summary>
        /// Gets or sets the condition of the case.
        /// </summary>
        public string When { get; set; }

        /// <summary>
        /// Gets or sets the results of this case when run through a <see cref="SwitchConverter"/>
        /// </summary>
        public object Then { get; set; }

        #endregion
        #region Construction.
        /// <summary>
        /// Switches the converter.
        /// </summary>
        public SwitchConverterCase()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchConverterCase"/> class.
        /// </summary>
        /// <param name="when">The condition of the case.</param>
        /// <param name="then">The results of this case when run through a <see cref="SwitchConverter"/>.</param>
        public SwitchConverterCase(string when, object then)
        {
            // Hook up the instances.
            this.Then = then;
            this.When = when;
        }
        #endregion

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"When={When}; Then={Then}";
        }
    }
}
