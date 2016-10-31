using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Deadfile.Tab.DesignTime
{
    class DesignTimeViewModel : IScreen
    {
        public string DisplayName { get; set; }
        public void Activate()
        {
            throw new NotImplementedException();
        }

        public bool IsActive { get; }
        public event EventHandler<ActivationEventArgs> Activated;
        public void Deactivate(bool close)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<DeactivationEventArgs> AttemptingDeactivation;
        public event EventHandler<DeactivationEventArgs> Deactivated;
        public void TryClose(bool? dialogResult = null)
        {
            throw new NotImplementedException();
        }

        public void CanClose(Action<bool> callback)
        {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyOfPropertyChange(string propertyName)
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public bool IsNotifying { get; set; }
    }
}
