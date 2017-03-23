using System;
using System.ComponentModel;
using Deadfile.Properties;
using MahApps.Metro.Controls;

namespace Deadfile
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : MetroWindow
    {
        public ShellView()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            this.SetPlacement(Settings.Default.MainWindowPlacement);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Settings.Default.MainWindowPlacement = this.GetPlacement();
            Settings.Default.Save();
        }
    }
}
