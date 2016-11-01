using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Tab.DesignTime;

namespace Deadfile.Tab.Home
{
    class HomePageDesignTimeViewModel : PageDesignTimeViewModel, IHomePageViewModel
    {
        public void Clients()
        {
            throw new NotImplementedException();
        }

        public void Jobs()
        {
            throw new NotImplementedException();
        }

        public void LocalAuthorities()
        {
            throw new NotImplementedException();
        }

        public Experience Experience
        {
            get { return Experience.Home; }
        }

        public bool ShowActionsPad { get; } = true;
    }
}
