using DSIO.Filters.Api.Sdk.Types.V1;
using Microsoft.Win32;
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

namespace WpfSample
{
    /// <summary>
    /// Interaction logic for ImageDataAndInfo.xaml
    /// </summary>
    public partial class ImageDataAndInfo : Window
    {
        public ImageDataAndInfo(MainViewModel mainViewModel)
        {
            DataContext = mainViewModel;
            InitializeComponent();

            // Initialize text box with sample ImageInfo Json data
            TbxImageInfo.Text = @"{
    'acquisitionInfo':
    {
        'binning': 'Unbinned'
    },
    'lutInfo':
    {
        'gamma': 2.3,
        'slope': 65535,
        'offset': 0,
        'totalGrays': 4096,
        'minimumGray': 3612,
        'maximumGray': 418
    }
}";

        }

        public MainViewModel ViewModel => DataContext as MainViewModel;

        private void BtnBrowseFileOpen_OnClick(object sender, RoutedEventArgs e)
        {
            // Use a FileDialog browser to allow user to select a PNG file
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png)|*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                ViewModel.UploadImageFileName = openFileDialog.FileName;
            }
        }

        private void BtnSubmit_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ViewModel.UploadImageFileName))
            {
                MessageBox.Show("Please select a image file to upload", "Image Upload");
                return;
            }

            try
            {
                // Convert the Json data to an ImageInfo
                // The sample allows the developer to enter Json data, but we need an ImageInfo
                // object to use with the SDK. So we try to deserialize the supplied Json now
                var imageInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<ImageInfo>(TbxImageInfo.Text);
                ViewModel.UploadImageInfo = imageInfo;

                this.DialogResult = true;
                this.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Json Deserialize");
            }
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            // Reset Upload parameters on cancel
            ViewModel.UploadImageFileName = null;
            ViewModel.UploadImageInfo = null;

            this.DialogResult = false;
            this.Close();
        }

    }
}
