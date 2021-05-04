using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using DSIO.Filters.Api.Sdk.Types.V1;
using Microsoft.Win32;

namespace WpfSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }

        public MainViewModel ViewModel => DataContext as MainViewModel;

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
        }

        private void BtnLogin_OnClick(object sender, RoutedEventArgs e)
        {
            // Try to login with credentials
            ViewModel.Login();
        }

        private void BtnGetImageDetails_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.GetImageDetails();
        }

        private void BtnCreateImage_OnClick(object sender, RoutedEventArgs e)
        {
            // Create a new image
            ViewModel.UploadImage(TbxFileName.Text.ToString());
        }

        private void BtnDeleteImage_OnClick(object sender, RoutedEventArgs e)
        {
            // delete image
            ViewModel.DeleteImage();
        }

        private void BtnBrowseFileOpen_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "image files (*.png)|*.png";
            if (openFileDialog.ShowDialog() == true)
                TbxFileName.Text = openFileDialog.FileName;
        }
    }
}
