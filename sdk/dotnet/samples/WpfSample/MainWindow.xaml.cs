using System.ComponentModel;
using System.Windows;
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
            ViewModel.UploadImage();
        }

        private void BtnCreateImageFromModalitySession_OnClick(object sender, RoutedEventArgs e)
        {
            // Create a new image
            ViewModel.CreateImageFromModalitySession();
        }

        private void BtnDeleteImage_OnClick(object sender, RoutedEventArgs e)
        {
            // delete image
            ViewModel.DeleteImage();
        }

        private void BtnSelectFilter_OnClick(object sender, RoutedEventArgs e)
        {
            // Select Filter
            ViewModel.SelectFilter();
        }

        private void BtnSupremeFilter_OnClick(object sender, RoutedEventArgs e)
        {
            // Supreme Filter
            ViewModel.SupremeFilter();
        }

        private void BtnAeFilter_OnClick(object sender, RoutedEventArgs e)
        {
            // Ae Filter
            ViewModel.AeFilter();
        }

        private void BtnUnmapFilter_OnClick(object sender, RoutedEventArgs e)
        {
            // Unmap Filter
            ViewModel.UnmapFilter();
        }

        private void BtnBrowseFileOpen_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "image files (*.png)|*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                ViewModel.UploadImageFileName = openFileDialog.FileName;
            }
        }
    }
}
