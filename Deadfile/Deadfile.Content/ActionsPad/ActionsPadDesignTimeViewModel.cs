using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Deadfile.Content.ActionsPad
{
    /// <summary>
    /// Base class to provide functionality that the actions pad will always have.
    /// </summary>
    class ActionsPadDesignTimeViewModel : IActionsPadViewModel
    {
        public ICommand EditItemCommand { get; } = null;
        public ICommand SaveItemCommand { get; } = null;
        public ICommand DeleteItemCommand { get; } = null;
        public ICommand DiscardItemCommand { get; } = null;
        public Visibility EditItemVisibility { get; } = Visibility.Visible;
        public Visibility SaveItemVisibility { get; } = Visibility.Hidden;
        public Visibility DiscardItemVisibility { get; } = Visibility.Hidden;
        public Visibility DeleteItemVisibility { get; } = Visibility.Visible;
    }
}
