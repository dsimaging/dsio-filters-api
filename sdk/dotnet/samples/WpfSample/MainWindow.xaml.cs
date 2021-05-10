using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;
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

        private async void BtnSelectFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SelectFilteredImage.Source = null;
            // Apply Select Filter and Update the displayed image using BitmapImage
            var stream = await ViewModel.SelectFilter();
            SelectFilteredImage.Source = stream.ToBitmapImage();
        }

        private async void BtnSupremeFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SupremeFilteredImage.Source = null;
            // Supreme Filter and Update the displayed image using BitmapImage
            var stream = await ViewModel.SupremeFilter();
            SupremeFilteredImage.Source = stream.ToBitmapImage();
        }

        private async void BtnAeFilter_OnClick(object sender, RoutedEventArgs e)
        {
            AeFilteredImage.Source = null;
            // Ae Filter and Update the displayed image using BitmapImage
            var stream = await ViewModel.AeFilter();
            AeFilteredImage.Source = stream.ToBitmapImage();
        }

        private async void BtnUnmapFilter_OnClick(object sender, RoutedEventArgs e)
        {
            UnmapFilteredImage.Source = null;
            // Unmap Filter and Update the displayed image using BitmapImage
            var stream = await ViewModel.UnmapFilter();
            UnmapFilteredImage.Source = stream.ToBitmapImage();
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
