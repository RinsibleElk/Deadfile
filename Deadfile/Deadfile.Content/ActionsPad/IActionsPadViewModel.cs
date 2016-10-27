using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Deadfile.Content.ActionsPad
{
    interface IActionsPadViewModel
    {
        ICommand EditItemCommand { get; }
        ICommand SaveItemCommand { get; }
        ICommand DeleteItemCommand { get; }
        ICommand DiscardItemCommand { get; }
        Visibility EditItemVisibility { get; }
        Visibility SaveItemVisibility { get; }
        Visibility DiscardItemVisibility { get; }
        Visibility DeleteItemVisibility { get; }
    }
}
