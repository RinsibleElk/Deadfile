using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Infrastructure.Interfaces;

namespace Deadfile.Tab.Actions
{
    interface IActionsPadViewModel
    {
        void EditItem();
        bool CanEditItem { get; }
        bool EditItemIsVisible { get; }
        void SaveItem();
        bool CanSaveItem { get; }
        bool SaveItemIsVisible { get; }
        void DeleteItem();
        bool CanDeleteItem { get; }
        bool DeleteItemIsVisible { get; }
        void DiscardItem();
        bool CanDiscardItem { get; }
    }
}
