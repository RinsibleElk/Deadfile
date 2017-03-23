using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Deadfile.Infrastructure.Styles;
using Deadfile.Model.Browser;
using Dragablz;

namespace Deadfile
{
    interface IShellViewModel
    {
        ItemActionCallback ClosingItemActionCallback { get; }
        IInterTabClient InterTabClient { get; }
        ICommand OpenNewTab { get; }
        ICommand OpenNewTabToBrowserModelCommand { get; }
        ICommand OpenNewTabToNewClientCommand { get; }
        ICommand OpenNewTabToExperienceCommand { get; }
        ICommand OpenNewTabToInvoiceClientCommand { get; }
        string Server { get; set; }
        string Database { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        ICommand AcceptCommand { get; }
        ICommand CancelCommand { get; }
        bool SettingsIsOpen { get; set; }
        Theme ThemeToUse { get; set; }
        Accent AccentToUse { get; set; }
    }
}
