using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Tab.Common;

namespace Deadfile.Tab.Home
{
    interface IHomePageViewModel : IPageViewModel
    {
        void Clients();
        void Jobs();
        void LocalAuthorities();
    }
}
