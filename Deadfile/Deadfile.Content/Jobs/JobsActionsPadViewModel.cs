using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Deadfile.Content.ActionsPad;
using Deadfile.Content.Events;
using Deadfile.Content.Interfaces;
using Deadfile.Content.Navigation;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Prism.Events;
using Prism.Regions;
using Deadfile.Content.ViewModels;
using Prism.Commands;

namespace Deadfile.Content.Jobs
{
    /// <summary>
    /// Clients page is simple, straightforward extension of the base actions pad, with additional action for Add.
    /// (Jobs and Invoices must be added through their respective parents.)
    /// </summary>
    sealed class JobsActionsPadViewModel : ActionsPadViewModel, IJobsActionsPadViewModel
    {
        public JobsActionsPadViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
        }
    }
}
