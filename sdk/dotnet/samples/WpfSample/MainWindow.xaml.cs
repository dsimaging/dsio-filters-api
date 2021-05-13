using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using WpfSample.Types;

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

        private async void BtnFilter_OnClick(object sender, RoutedEventArgs e)
        {
            Stream stream = null;

            FilteredImage.Source = null;

            // Make sure we have an image resource to process
            if (string.IsNullOrEmpty(ViewModel.ImageResourceId))
            {
                MessageBox.Show("Please create or retrieve an image resource to filter");
                return;
            }

            try
            {
                switch (ViewModel.SelectedFilterParam)
                {
                    case FilterType.Select:
                    {
                        // Apply Select Filter and Update the displayed image using BitmapImage
                        stream = await ViewModel.SelectFilter();
                        break;
                    }
                    case FilterType.Supreme:
                    {
                        // Apply Supreme Filter and Update the displayed image using BitmapImage
                        stream = await ViewModel.SupremeFilter();
                        break;
                    }
                    case FilterType.Ae:
                    {
                        // Apply Ae Filter and Update the displayed image using BitmapImage
                        stream = await ViewModel.AeFilter();
                        break;
                    }
                    case FilterType.Unmap:
                    {
                        // Apply Unmap Filter and Update the displayed image using BitmapImage
                        stream = await ViewModel.UnmapFilter();
                        break;
                    }
                }

                if (stream != null)
                {
                    FilteredImage.Source = stream.ToBitmapImage();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

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
