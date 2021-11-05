using System.IO;
using System.Windows;
using System.Net.Http;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Collections.Generic;
using Newtonsoft.Json;

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

        private string _imageResourceId;
        public string ImageResourceId
        {
            get => _imageResourceId;
            set
            {
                if (value != _imageResourceId)
                {
                    _imageResourceId = value;
                    OnPropertyChanged();
                }
            }
        }

        private ImageInfo _uploadImageInfo;
        public ImageInfo UploadImageInfo
        {
            get => _uploadImageInfo;
            set
            {
                if (value != _uploadImageInfo)
                {
                    _uploadImageInfo = value;
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

        private string _modalitySessionId;
        public string ModalitySessionId
        {
            get => _modalitySessionId;
            set
            {
                if (value != _modalitySessionId)
                {
                    _modalitySessionId = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _modalityImageId;
        public string ModalityImageId
        {
            get => _modalityImageId;
            set
            {
                if (value != _modalityImageId)
                {
                    _modalityImageId = value;
                    OnPropertyChanged();
                }
            }
        }

        private ImageResource _selectedImageResource;
        public ImageResource SelectedImageResource
        {
            get => _selectedImageResource;
            set
            {
                if (value != _selectedImageResource)
                {
                    _selectedImageResource = value;
                    OnPropertyChanged();
                }
            }
        }

        public void UploadImage()
        {
            ImageDataAndInfo imageDataAndInfo = new ImageDataAndInfo(this);
            var ret = imageDataAndInfo.ShowDialog();

            if (!string.IsNullOrEmpty(UploadImageFileName) && UploadImageInfo != null)
            {
                var fileStream = new FileStream(UploadImageFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                var imageContent = new StreamContent(fileStream);

                // Upload a new image
                _serviceProxy.UploadImage(UploadImageInfo, imageContent, "image/png").ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        MessageBox.Show(task.Exception?.Message);
                    }
                    else if (task.IsCompleted)
                    {
                        SelectedImageResource = task.Result;
                        ImageResourceId = SelectedImageResource.Id;
                    }
                });
            }
        }

        public void CreateImageFromModalitySession()
        {
            if (!string.IsNullOrEmpty(ModalitySessionId) && !string.IsNullOrEmpty(ModalityImageId))
            {
                // Create ModalitySession from supplied session id and image id
                var modalitySession = new ModalitySession()
                {
                    SessionId = ModalitySessionId,
                    ImageId = ModalityImageId
                };

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
                        ImageResourceId = SelectedImageResource.Id;
                    }
                });
            }
        }

        // Get Image Details
        public void GetImageDetails()
        {
            if (string.IsNullOrEmpty(ImageResourceId))
            {
                MessageBox.Show("Please provide a resource Id to retrieve");
                return;
            }

            SelectedImageResource = null;
            Mouse.OverrideCursor = Cursors.Wait;

            // Get Image Details
            _serviceProxy.GetImage(ImageResourceId).ContinueWith(task =>
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

        /// <summary>
        /// Select Filter
        /// </summary>
        public async Task<Stream> SelectFilter()
        {
            if (SelectedImageResource != null)
            {
                // Apply Select Filter
                SelectFilterImageParam selectFilterImageParam = Newtonsoft.Json.JsonConvert.DeserializeObject<SelectFilterImageParam>(FilterParam);
                return await _serviceProxy.SelectFilter(SelectedImageResource.Id, selectFilterImageParam);
            }

            return null;
        }

        /// <summary>
        /// Supreme Filter
        /// </summary>
        public async Task<Stream> SupremeFilter()
        {
            if (SelectedImageResource != null)
            {
                SupremeFilterImageParam supremeFilterImageParam = Newtonsoft.Json.JsonConvert.DeserializeObject<SupremeFilterImageParam>(FilterParam);
                // Apply Supreme Filter
                return await _serviceProxy.SupremeFilter(SelectedImageResource.Id, supremeFilterImageParam);
            }

            return null;
        }

        /// <summary>
        /// Ae Filter
        /// </summary>
        public async Task<Stream> AeFilter()
        {
            if (SelectedImageResource != null)
            {
                OmegaFilterImageParam omegaFilterImageParam = Newtonsoft.Json.JsonConvert.DeserializeObject<OmegaFilterImageParam>(FilterParam);
                // Apply Ae Filter
                return await _serviceProxy.AeFilter(SelectedImageResource.Id, omegaFilterImageParam);
            }

            return null;
        }

        /// <summary>
        /// Unmap Filter
        /// </summary>
        public async Task<Stream> UnmapFilter()
        {
            if (SelectedImageResource != null)
            {
                LutInfo lutInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<LutInfo>(FilterParam);
                // Apply Unmap Filter
                return await _serviceProxy.UnmapFilter(SelectedImageResource.Id, lutInfo);
            }

            return null;
        }

        #endregion
    }
}
