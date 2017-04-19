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

namespace CalendarImpl {
    /// <summary>
    /// Interaction logic for TestUserControl.xaml
    /// </summary>
    public partial class TestUserControl : UserControl {
        public Window ParentWindow
        {
            get {
                DependencyObject parent = Parent;
                do {
                    if (parent is Window) {
                        return parent as Window;
                    }
                    // hopefully the parent may be a FrameworkElement ;-)
                    parent = (parent as FrameworkElement)?.Parent;
                } while (parent != null);
                return null;
            }
        }

        public TestUserControl() {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            var parent = ParentWindow;
            if (parent != null) {
                parent.DialogResult = true;
                parent.Close();
            }
        }
    }
}
