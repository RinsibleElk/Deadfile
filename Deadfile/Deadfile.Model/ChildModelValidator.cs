using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model
{
    public static class ChildModelValidator
    {
        public static ValidationResult ChildrenAreValid(ObservableCollection<ChildModelBase> children, ValidationContext context)
        {
            foreach (var childModelBase in children)
            {
                if (childModelBase.HasErrors)
                    return new ValidationResult("A child has errors", new string[] { "Children" });
            }
            return ValidationResult.Success;
        }
    }
}
