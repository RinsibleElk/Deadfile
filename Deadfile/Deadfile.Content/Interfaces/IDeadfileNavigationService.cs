using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Content.Navigation;

namespace Deadfile.Content.Interfaces
{
    /// <summary>
    /// This is just to move the usage of the RegionManager and Uris into a single place.
    /// </summary>
    public interface IDeadfileNavigationService
    {
        void NavigateTo(Experience experience);
    }
}
