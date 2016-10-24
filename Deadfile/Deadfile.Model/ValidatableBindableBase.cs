using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace Deadfile.Model
{
    public class ValidatableBindableBase : BindableBase, INotifyDataErrorInfo, INotifyPropertyChanging
    {
        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        public event EventHandler<DataErrorsChangedEventArgs>
           ErrorsChanged = delegate { };

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
                return _errors[propertyName];
            else
                return null;
        }

        public bool HasErrors
        {
            get { return _errors.Count > 0; }
        }

        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals((object)storage, (object)value)) return false;
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
            base.SetProperty<T>(ref storage, value, propertyName);
            ValidateProperty(propertyName, value);
            return true;
        }

        /// <summary>
        /// Notifies listeners that a property value may change.
        /// </summary>
        /// <param name="propertyName">
        /// Name of the property used to notify listeners. This value is optional and can be provided automatically when
        /// invoked from compilers that support <see cref="T:System.Runtime.CompilerServices.CallerMemberNameAttribute" />.
        /// </param>
        protected virtual void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            // ISSUE: reference to a compiler-generated field
            PropertyChangingEventHandler propertyChanging = this.PropertyChanging;
            if (propertyChanging == null)
                return;
            PropertyChangingEventArgs e = new PropertyChangingEventArgs(propertyName);
            propertyChanging((object)this, e);
        }

        private void ValidateProperty<T>(string propertyName, T value)
        {
            var results = new List<ValidationResult>();

            ValidationContext context = new ValidationContext(this); 
            context.MemberName = propertyName;
            Validator.TryValidateProperty(value, context, results);

            if (results.Any())
            {
                _errors[propertyName] = results.Select(c => c.ErrorMessage).ToList(); 
            }
            else
            {
                _errors.Remove(propertyName);
            }

            ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public event PropertyChangingEventHandler PropertyChanging;
    }
}