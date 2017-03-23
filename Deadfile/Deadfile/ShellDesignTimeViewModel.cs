using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Infrastructure.Styles;
using Dragablz;

namespace Deadfile
{
    class ShellDesignTimeViewModel : IShellViewModel
    {
        public ItemActionCallback ClosingItemActionCallback { get; } = null;
        public IInterTabClient InterTabClient { get; } = null;
        public ICommand OpenNewTab { get; } = null;
        public ICommand OpenNewTabToBrowserModelCommand { get; } = null;
        public ICommand OpenNewTabToNewClientCommand { get; } = null;
        public ICommand OpenNewTabToExperienceCommand { get; } = null;
        public ICommand OpenNewTabToInvoiceClientCommand { get; } = null;
        public string Server { get; set; } = @".\SQLEXPRESS";
        public string Database { get; set; } = "Deadfile";
        public string Username { get; set; } = "RinsibleElk";
        public string Password { get; set; } = "N0t4R34LP455w0rD";
        public ICommand AcceptCommand { get; } = null;
        public ICommand CancelCommand { get; } = null;
        public bool SettingsIsOpen { get; set; } = false;
        public Theme ThemeToUse { get; set; } = Theme.BaseDark;
        public Accent AccentToUse { get; set; } = Accent.Red;
    }
}
