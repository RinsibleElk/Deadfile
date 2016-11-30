using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Core;

namespace Deadfile.Model
{
    public abstract class ModelBase : ValidatableBindableBase
    {
        public ModelBase()
        {
            Id = NewModelId;
        }
        // Invalid value for the db.
        public const int NewModelId = Int32.MinValue;
        public abstract int Id { get; set; }
        public bool DisableUndoTracking { get; set; }
    }
}
