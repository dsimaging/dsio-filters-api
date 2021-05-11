using System.IO;
using System.Windows;
using System.Net.Http;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Collections.Generic;

using DSIO.Filters.Api.Sdk.Client.V1;
using DSIO.Filters.Api.Sdk.Types.V1;
using WpfSample.Types;
using WpfSample.Defaults;

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

        private void SetDefaultValues()
        {
            FilterParamList = FilterDefaults.GetValues();
            // assign Select Filter on load
            SelectedFilterParam = FilterType.Select;
            FilterParam = FilterParamList[FilterType.Select];
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

        private Dictionary<FilterType, string> _filterParamList;
        public Dictionary<FilterType, string> FilterParamList
        {
            get => _filterParamList;
            set
            {
                _filterParamList = value;
                OnPropertyChanged();
            }
        }

        private FilterType _selectedFilterParam;
        public FilterType SelectedFilterParam
        {
            get => _selectedFilterParam;
            set
            {
                _selectedFilterParam = value;
                FilterParam = FilterParamList[_selectedFilterParam];
                OnPropertyChanged();
            }
        }

        private string _filterParam;
        public string FilterParam
        {
            get => _filterParam;
            set
            {
                if (value != _filterParam)
                {
                    _filterParam = value;
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

        #region Images

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

        #region Filters

        /// <summary>
        /// Select Filter
        /// </summary>
        public async Task<Stream> SelectFilter()
        {
            // Apply Select Filter
            SelectFilterImageParam selectFilterImageParam = Newtonsoft.Json.JsonConvert.DeserializeObject<SelectFilterImageParam>(FilterParam);
            return await _serviceProxy.SelectFilter(ImageId, selectFilterImageParam);
        }

        /// <summary>
        /// Supreme Filter
        /// </summary>
        public async Task<Stream> SupremeFilter()
        {
            SupremeFilterImageParam supremeFilterImageParam = Newtonsoft.Json.JsonConvert.DeserializeObject<SupremeFilterImageParam>(FilterParam);
            // Apply Supreme Filter
            return await _serviceProxy.SupremeFilter(ImageId, supremeFilterImageParam);
        }

        /// <summary>
        /// Ae Filter
        /// </summary>
        public async Task<Stream> AeFilter()
        {
            OmegaFilterImageParam omegaFilterImageParam = Newtonsoft.Json.JsonConvert.DeserializeObject<OmegaFilterImageParam>(FilterParam);
            // Apply Ae Filter
            return await _serviceProxy.AeFilter(ImageId, omegaFilterImageParam);
        }

        /// <summary>
        /// Unmap Filter
        /// </summary>
        public async Task<Stream> UnmapFilter()
        {
            LutInfo lutInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<LutInfo>(FilterParam);
            // Apply Unmap Filter
            return await _serviceProxy.UnmapFilter(ImageId, lutInfo);
        }

        #endregion
    }
}
