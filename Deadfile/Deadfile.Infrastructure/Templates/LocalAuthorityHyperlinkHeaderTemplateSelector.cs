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
    /// Select HeaderTemplate for an Expander based on whether there is a URL set in a <see cref="LocalAuthorityModel"/>.
    /// </summary>
    public class LocalAuthorityHyperlinkHeaderTemplateSelector : DataTemplateSelector
    {
        public DataTemplate WithUrlTemplate { get; set; }
        public DataTemplate NoUrlTemplate { get; set; }
        public DataTemplate AddNewTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var localAuthority = (item as LocalAuthorityModel);
            if (localAuthority == null || localAuthority.Id == ModelBase.NewModelId) return AddNewTemplate;
            if (!localAuthority.HasUrl) return NoUrlTemplate;
            return WithUrlTemplate;
        }
    }
}
