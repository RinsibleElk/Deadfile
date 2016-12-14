using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model
{
    public abstract class JobChildModelBase : ModelBase
    {
        private int _jobId;
        public int JobId
        {
            get { return _jobId; }
            set { SetProperty(ref _jobId, value); }
        }

        private int _clientId;
        public int ClientId
        {
            get { return _clientId; }
            set { SetProperty(ref _clientId, value); }
        }
    }
}
