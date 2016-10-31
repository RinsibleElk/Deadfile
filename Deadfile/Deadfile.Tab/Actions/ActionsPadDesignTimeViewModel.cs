using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Tab.Actions
{
    class ActionsPadDesignTimeViewModel : IActionsPadViewModel
    {
        public void EditItem()
        {
            throw new NotImplementedException();
        }

        public bool CanEditItem { get; } = true;
        public bool EditItemIsVisible { get; } = true;

        public void SaveItem()
        {
            throw new NotImplementedException();
        }

        public bool CanSaveItem { get; } = false;
        public bool SaveItemIsVisible { get; } = false;

        public void DeleteItem()
        {
            throw new NotImplementedException();
        }

        public bool CanDeleteItem { get; } = true;
        public bool DeleteItemIsVisible { get; } = true;

        public void DiscardItem()
        {
            throw new NotImplementedException();
        }

        public bool CanDiscardItem { get; } = false;
        public bool DiscardItemIsVisible { get; } = false;
    }
}
