﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model.Billable
{
    /// <summary>
    /// Leaf billable item representing a billable hour for a particular job.
    /// </summary>
    public class BillableBillableHour : BillableModel
    {
        public override BillableModelType ModelType
        {
            get { return BillableModelType.BillableHour; }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                if (SetProperty(ref _description, value))
                    OnPropertyChanged(nameof(Text));
            }
        }

        public override string Text { get { return Description + " (" + NetAmount + ")"; } }
    }
}
