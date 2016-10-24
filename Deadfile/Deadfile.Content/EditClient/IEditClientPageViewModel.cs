using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Content.Interfaces;
using Deadfile.Model;

namespace Deadfile.Content.EditClient
{
    /// <summary>
    /// ViewModel used for editing and creating clients.
    /// </summary>
    public interface IEditClientPageViewModel : IDeadfileViewModel
    {
        ClientModel ClientModelUnderEdit { get; set; }
    }
}
