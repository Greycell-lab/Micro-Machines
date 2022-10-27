using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Micro_Machines
{
    /// <summary>
    /// Interaktionslogik für GameOver.xaml
    /// </summary>
    public partial class GameOver : Window
    {
        public GameOver()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(sender == Yes)
            {
                Application.Current.Shutdown();
                Process.Start(Application.ResourceAssembly.Location);
                
            }
            if(sender == No)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
