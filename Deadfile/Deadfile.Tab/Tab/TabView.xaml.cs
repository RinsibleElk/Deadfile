using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Deadfile.Tab.Tab
{
    /// <summary>
    /// Interaction logic for TabView.xaml
    /// </summary>
    public partial class TabView : UserControl
    {
        public TabView()
        {
            InitializeComponent();
        }

        private static readonly GridLength Zero = new GridLength(0);
        private GridLength? _browserPaneWidth;
        private void GridSplitter_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (BrowserPane.Visibility == Visibility.Visible)
            {
                BrowserPane.Visibility = Visibility.Collapsed;
                _browserPaneWidth = MainGrid.ColumnDefinitions[2].Width;
                MainGrid.ColumnDefinitions[2].Width = Zero;
                GridSplitter.Width = 10;
                GridSplitter.ToolTip = "Double Click to Expand Browser Pane";
            }
            else if (_browserPaneWidth != null)
            {
                BrowserPane.Visibility = Visibility.Visible;
                MainGrid.ColumnDefinitions[2].Width = _browserPaneWidth.Value;
                _browserPaneWidth = null;
                GridSplitter.Width = 3;
                GridSplitter.ToolTip = "Double Click to Collapse";
            }
        }
    }
}
