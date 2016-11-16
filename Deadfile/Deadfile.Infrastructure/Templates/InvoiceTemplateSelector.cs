using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Deadfile.Entity;
using Deadfile.Model;

namespace Deadfile.Infrastructure.Templates
{
    public class InvoiceTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PksTemplate { get; set; }
        public DataTemplate I3DTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return PksTemplate;
            var company = (Company) item;
            if (company == Company.PaulSamsonCharteredSurveyorLtd) return PksTemplate;
            return I3DTemplate;
        }
    }
}
