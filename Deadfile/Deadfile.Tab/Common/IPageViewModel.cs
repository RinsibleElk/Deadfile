using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Deadfile.Infrastructure.Interfaces;

namespace Deadfile.Tab.Common
{
    public interface IPageViewModel : IScreen, IJournaled
    {
        Experience Experience { get; }
    }
}
