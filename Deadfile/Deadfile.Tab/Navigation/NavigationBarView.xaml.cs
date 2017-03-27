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

namespace Deadfile.Tab.Navigation
{
    /// <summary>
    /// Interaction logic for NavigationBarView.xaml
    /// </summary>
    public partial class NavigationBarView : UserControl
    {
        public NavigationBarView()
        {
            InitializeComponent();

            this.Loaded += NavigationBarViewLoaded;
        }

        private void NavigationBarViewLoaded(object sender, RoutedEventArgs e)
        {
            Window w = Window.GetWindow(SearchTextBox);
            if (w != null)
            {
                w.LocationChanged += (o, args) =>
                {
                    var offset = SearchPopup.HorizontalOffset;
                    SearchPopup.HorizontalOffset = offset + 1;
                    SearchPopup.HorizontalOffset = offset;
                };
                w.SizeChanged += (o, args) =>
                {
                    var offset = SearchPopup.HorizontalOffset;
                    SearchPopup.HorizontalOffset = offset + 1;
                    SearchPopup.HorizontalOffset = offset;
                };
            }
        }
    }
}
