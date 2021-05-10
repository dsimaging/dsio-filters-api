using System.IO;
using System.Windows;
using System.Net.Http;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using DSIO.Filters.Api.Sdk.Client.V1;
using DSIO.Filters.Api.Sdk.Types.V1;
using System.Windows.Input;
using System.Windows.Media.Imaging;

/// <summary>
/// ViewModel class for MainWindow
/// </summary>
/// 
namespace WpfSample
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ServiceProxy _serviceProxy;

        public MainViewModel()
        {
            _serviceProxy = new ServiceProxy();
            SetDefaultValues();
        }
        void SetDefaultValues()
        {
            SelectFilterParam = @"{
'enhancementMode': 'edgePro',
'lutInfo': {
                'gamma': 2.3,
'slope': 65535,
'offset': 0,
'totalGrays': 4096,
'minimumGray': 3612,
'maximumGray': 418
}
        }";

            SupremeFilterParam = @"{
  'task': 'general',
  'binningMode': 'binned2X2',
  'sharpness': 70,
  'lutInfo': {
                'gamma': 2.3,
    'slope': 65535,
    'offset': 0,
    'totalGrays': 4096,
    'minimumGray': 3612,
    'maximumGray': 418
  }
        }";

            OmegaFilterParam = @"{
  'task': 'general',
  'sharpness': 70,
  'lutInfo': {
                'gamma': 2.3,
    'slope': 65535,
    'offset': 0,
    'totalGrays': 4096,
    'minimumGray': 3612,
    'maximumGray': 418
  }
        }";

            UnmapFilterParam = @"{
  'gamma': 2.3,
  'slope': 65535,
  'offset': 0,
  'totalGrays': 4096,
  'minimumGray': 3612,
  'maximumGray': 418
}";
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Login

        private string _apiUserName;
        public string ApiUserName
        {
            get => _apiUserName;
            set
            {
                if (value != _apiUserName)
                {
                    _apiUserName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _apiKey;
        public string ApiKey
        {
            get => _apiKey;
            set
            {
                if (value != _apiKey)
                {
                    _apiKey = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isAuthenticated;
        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            private set
            {
                if (value != _isAuthenticated)
                {
                    _isAuthenticated = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _loginStatus;
        public string LoginStatus
        {
            get => _loginStatus;
            set
            {
                if (value != _loginStatus)
                {
                    _loginStatus = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _imageId;
        public string ImageId
        {
            get => _imageId;
            set
            {
                if (value != _imageId)
                {
                    _imageId = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _uploadImageFileName;
        public string UploadImageFileName
        {
            get => _uploadImageFileName;
            set
            {
                if (value != _uploadImageFileName)
                {
                    _uploadImageFileName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _modalitySessionParam;
        public string ModalitySessionParam
        {
            get => _modalitySessionParam;
            set
            {
                if (value != _modalitySessionParam)
                {
                    _modalitySessionParam = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _selectFilterParam;
        public string SelectFilterParam
        {
            get => _selectFilterParam;
            set
            {
                if (value != _selectFilterParam)
                {
                    _selectFilterParam = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _supremeFilterParam;
        public string SupremeFilterParam
        {
            get => _supremeFilterParam;
            set
            {
                if (value != _supremeFilterParam)
                {
                    _supremeFilterParam = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _omegaFilterParam;
        public string OmegaFilterParam
        {
            get => _omegaFilterParam;
            set
            {
                if (value != _omegaFilterParam)
                {
                    _omegaFilterParam = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _unmapFilterParam;
        public string UnmapFilterParam
        {
            get => _unmapFilterParam;
            set
            {
                if (value != _unmapFilterParam)
                {
                    _unmapFilterParam = value;
                    OnPropertyChanged();
                }
            }
        }

 
        public void Login()
        {
            // Update status
            LoginStatus = "Logging in...";
            IsAuthenticated = false;

            // configure credentials
            _serviceProxy.SetBasicAuthenticationHeader(ApiUserName, ApiKey);

            // Check availability to confirm credentials
            _serviceProxy.IsServiceAvailable().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    // Show the error message
                    LoginStatus = "Error";
                    MessageBox.Show(task.Exception?.Message);
                }
                else if (task.IsCompleted)
                {
                    IsAuthenticated = task.Result;
                    LoginStatus = IsAuthenticated ? "Login Successful" : "Service is unavailable";
                }
            });
        }

        #endregion

        #region Image
        private ImageResource _selectedImageResource;
        public ImageResource SelectedImageResource
        {
            get => _selectedImageResource;
            set
            {
                if (value != _selectedImageResource)
                {
                    _selectedImageResource = value;
                    ImageId = string.Empty;
                    if (SelectedImageResource != null)
                    {
                        ImageId = SelectedImageResource.Id;
                    }
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Images
        public void UploadImage()
        {
            var fileStream = new FileStream(UploadImageFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            var imageContent = new StreamContent(fileStream);
            // Upload a new image
            _serviceProxy.UploadImage(imageContent, "image/png").ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    MessageBox.Show(task.Exception?.Message);
                }
                else if (task.IsCompleted)
                {
                    SelectedImageResource = task.Result;
                }
            });
        }

        public void CreateImageFromModalitySession()
        {
            ModalitySession modalitySession = Newtonsoft.Json.JsonConvert.DeserializeObject<ModalitySession>(ModalitySessionParam);
            // Create image from Modality Session
            _serviceProxy.CreateImage(modalitySession).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    MessageBox.Show(task.Exception?.Message);
                }
                else if (task.IsCompleted)
                {
                    SelectedImageResource = task.Result;
                }
            });
        }

        // Get Image Details
        public void GetImageDetails()
        {
            SelectedImageResource = null;
            Mouse.OverrideCursor = Cursors.Wait;

            // Get Image Details
            _serviceProxy.GetImage(ImageId).ContinueWith(task =>
            {
            // Must be on UI thread to change Mouse
            Mouse.OverrideCursor = null;
                if (task.IsFaulted)
                {
                    MessageBox.Show(task.Exception?.Message);
                }
                else if (task.IsCompletedSuccessfully)
                {
                    SelectedImageResource = task.Result;
                }
            // We synchronize the Continuation task so we can make UI changes
        }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void DeleteImage()
        {
            if (SelectedImageResource != null)
            {
                // Delete Image
                _serviceProxy.DeleteImage(SelectedImageResource.Id)
                    .ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            MessageBox.Show(task.Exception?.Message);
                        }
                        else
                        {
                            // Cleanup image data
                            SelectedImageResource = null;
                        }
                    });
            }
        }
        #endregion

        #region "Filters"
        /// <summary>
        /// Select Filter
        /// </summary>
        public async Task<Stream> SelectFilter()
        {
            // Apply Select Filter
            SelectFilterImageParam selectFilterImageParam = Newtonsoft.Json.JsonConvert.DeserializeObject<SelectFilterImageParam>(SelectFilterParam);
            return await _serviceProxy.SelectFilter(ImageId, selectFilterImageParam);
        }

        /// <summary>
        /// Supreme Filter
        /// </summary>
        public async Task<Stream> SupremeFilter()
        {
            SupremeFilterImageParam supremeFilterImageParam = Newtonsoft.Json.JsonConvert.DeserializeObject<SupremeFilterImageParam>(SupremeFilterParam);
            // Apply Supreme Filter
            return await _serviceProxy.SupremeFilter(ImageId, supremeFilterImageParam);
        }

        /// <summary>
        /// Ae Filter
        /// </summary>
        public async Task<Stream> AeFilter()
        {
            OmegaFilterImageParam omegaFilterImageParam = Newtonsoft.Json.JsonConvert.DeserializeObject<OmegaFilterImageParam>(OmegaFilterParam);
            // Apply Ae Filter
            return await _serviceProxy.AeFilter(ImageId, omegaFilterImageParam);
        }

        /// <summary>
        /// Unmap Filter
        /// </summary>
        public async Task<Stream> UnmapFilter()
        {
            LutInfo lutInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<LutInfo>(UnmapFilterParam);
            // Apply Unmap Filter
            return await _serviceProxy.UnmapFilter(ImageId, lutInfo);
        }

        #endregion
    }
}
