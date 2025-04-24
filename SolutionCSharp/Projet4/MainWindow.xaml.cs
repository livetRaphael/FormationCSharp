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
using System.Drawing;

namespace Projet4
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

        private ImageBrush TransformUrlToBrush(string url)
        {
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri(url));
            brush.Stretch = Stretch.Fill;
            return brush;
        }
        public void ChangePictures()
        {
            btnRight.Background = TransformUrlToBrush("ms-appx:///Assets/Un-plat-familial-en-sauce-comme-chez-mamie.jpg");
           
            btnLeft.Background = TransformUrlToBrush("ms-appx:///Assets/malkha-plat-marocain.jpg");
        }

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Click");
            ChangePictures();
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            ChangePictures();
        }
    }
}
