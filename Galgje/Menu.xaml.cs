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
using System.Windows.Shapes;

namespace Galgje
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void buttonMultiPlayer_Click(object sender, RoutedEventArgs e)
        {
            Multi multi = new Multi();
            multi.Show();
            this.Close();
        }

        private void buttonSinglePlayer_Click(object sender, RoutedEventArgs e)
        {
            Single multi = new Single();
            multi.Show();
            this.Close();
        }
    }
}
