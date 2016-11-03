using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Deadfile.Model;

namespace Deadfile.Infrastructure.Templates
{
    /// <summary>
    /// Header template selector for an expander that chooses an appropriate expander header template for a new model.
    /// </summary>
    public class ModelTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ExistingTemplate { get; set; }
        public DataTemplate AddNewTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var model = (item as ModelBase);
            if (model == null || model.Id == ModelBase.NewModelId) return AddNewTemplate;
            return ExistingTemplate;
        }
    }
}
