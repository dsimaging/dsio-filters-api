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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "image files (*.png)|*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                ViewModel.UploadImageFileName = openFileDialog.FileName;
            }
        }
        private void BtnSubmit_OnClick(object sender, RoutedEventArgs e)
        {
            var imageInfoJson = TbxImageInfo.Text;
            var imageInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<ImageInfo>(imageInfoJson);
            ViewModel.UploadImageInfo = imageInfo;
            this.DialogResult = true;
            this.Close();
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

    }
}
