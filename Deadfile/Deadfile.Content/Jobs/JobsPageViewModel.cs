﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Deadfile.Content.Events;
using Deadfile.Content.Interfaces;
using Deadfile.Content.Navigation;
using Deadfile.Content.Undo;
using Deadfile.Content.ViewModels;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Prism.Events;
using Prism.Regions;

namespace Deadfile.Content.Jobs
{
    public class JobsPageViewModel : EditableItemContentViewModelBase<JobModel>, IJobsPageViewModel
    {
        private readonly IDeadfileRepository _repository;

        public JobsPageViewModel(
            IEventAggregator eventAggregator,
            IDeadfileNavigationService navigationService,
            IDeadfileRepository repository,
            INavigationParameterMapper mapper)
            : base(eventAggregator, navigationService, mapper)
        {
            _repository = repository;
        }

        public override Experience Experience
        {
            get { return Experience.Jobs; }
        }

        public override void PerformSave()
        {
            try
            {
//                _repository.SaveClient(SelectedItem);
            }
            catch (Exception)
            {
                //TODO Do something. Like raise a dialog box or something. Then clean up.
                throw;
            }
        }

        public override JobModel GetModelById(int id)
        {
            return _repository.GetJobById(id);
        }
    }
}