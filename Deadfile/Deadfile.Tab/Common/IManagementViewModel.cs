using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Model;

namespace Deadfile.Tab.Common
{
    /// <summary>
    /// Represents a simple item for management, with a relatively loose relationship to other entities in the database, managed in its own page.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IManagementViewModel<T> : ISimpleEditableItemViewModel<T>, IPageViewModel
        where T : ModelBase
    {
    }
}
