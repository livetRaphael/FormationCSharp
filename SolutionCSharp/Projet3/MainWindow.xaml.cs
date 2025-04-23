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

namespace Projet3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        private void Button_Click_Net(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Votre logiciel est nettoyé !");
        }

        private void Button_Click_MAJ(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_Histo(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_Web(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
