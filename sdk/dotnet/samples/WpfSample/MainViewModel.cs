using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DSIO.Filters.Api.Sdk.Types.V1;
using DSIO.Filters.Api.Sdk.Client.V1;
using System.IO;
using System.Net.Http;

namespace WpfSample
{
    /// <summary>
    /// ViewModel class for MainWindow
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ServiceProxy _serviceProxy;
        //private ISubscription _acquisitionStatusSubscription;

        public MainViewModel()
        {
            _serviceProxy = new ServiceProxy();
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

        #region Device

        //private ObservableCollection<DeviceInfo> _devices;

        //public ObservableCollection<DeviceInfo> Devices
        //{
        //    get => _devices;
        //    private set
        //    {
        //        _devices = value;
        //        OnPropertyChanged();
        //    }
        //}

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

        // Must be called from UI thread to properly update Devices list and Selected Device
        public void GetImageDetails()
        {
            // Disable current selection
            SelectedImageResource = null;
            Mouse.OverrideCursor = Cursors.Wait;

            // Get all devices
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

        #endregion

        #region Images

        //private AcquisitionSession _session;
        //public AcquisitionSession Session
        //{
        //    get => _session;
        //    set
        //    {
        //        if (value != _session)
        //        {
        //            _session = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //private AcquisitionInfo _acquisitionInfo;
        //public AcquisitionInfo AcquisitionInfo
        //{
        //    get => _acquisitionInfo;
        //    set
        //    {
        //        if (value != _acquisitionInfo)
        //        {
        //            _acquisitionInfo = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //private AcquisitionStatus _acquisitionStatus;
        //public AcquisitionStatus AcquisitionStatus
        //{
        //    get => _acquisitionStatus;
        //    set
        //    {
        //        if (value != _acquisitionStatus)
        //        {
        //            _acquisitionStatus = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //private ObservableCollection<ImageInfo> _images;

        //public ObservableCollection<ImageInfo> Images
        //{
        //    get => _images;
        //    set
        //    {
        //        if (value != _images)
        //        {
        //            _images = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //private ImageInfo _selectedImage;

        //public ImageInfo SelectedImage
        //{
        //    get => _selectedImage;
        //    set
        //    {
        //        if (value != _selectedImage)
        //        {
        //            _selectedImage = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        public void UploadImage(string imageFileName)
        {
            var fileStream = new FileStream(imageFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            var imageContent = new StreamContent(fileStream);
            // Create the session
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
                    });

                // Cleanup image data
                SelectedImageResource = null;
            }
        }

        //public void UpdateAcquisitionInfo()
        //{
        //    // Set AcquisitionInfo for the next exposure using the
        //    // AcquisitionInfo property of our ViewModel
        //    if (Session != null && AcquisitionInfo != null)
        //    {
        //        _serviceProxy.UpdateAcquisitionInfo(Session.SessionId, AcquisitionInfo)
        //            .ContinueWith(task =>
        //            {
        //                if (task.IsFaulted)
        //                {
        //                    MessageBox.Show(task.Exception?.Message);
        //                }
        //                else if (task.IsCompleted)
        //                {
        //                    AcquisitionInfo = task.Result;
        //                }
        //            });
        //    }
        //}

        // Must be called from UI thread to properly update Images and SelectedImage
        //public void UpdateImageList(string selectImageId = null)
        //{
        //    // Update our collection of images
        //    if (Session != null)
        //    {
        //        _serviceProxy.GetAllImages(Session.SessionId)
        //            .ContinueWith(task =>
        //            {
        //                if (task.IsFaulted)
        //                {
        //                    MessageBox.Show(task.Exception?.Message);
        //                }
        //                else if (task.IsCompleted)
        //                {
        //                    Images = new ObservableCollection<ImageInfo>(task.Result);

        //                    // try to reselect the SelectedImage
        //                    if (!string.IsNullOrEmpty(selectImageId))
        //                        SelectedImage = Images.FirstOrDefault(info => info.Id == selectImageId);
        //                }
        //            }, TaskScheduler.FromCurrentSynchronizationContext());
        //    }
        //}

        #endregion

        /// <summary>
        /// This callback is used to process current AcquisitionStatus from our subscription
        /// </summary>
        //private void ProcessAcquisitionStatus(AcquisitionStatus status)
        //{
        //    AcquisitionStatus = status;
        //    if (status.State == AcquisitionStatus.AcquisitionState.NoAcquisitionInfo)
        //    {
        //        // Create a new instance of AcquisitionInfo so we can fill in properties
        //        AcquisitionInfo = new AcquisitionInfo();
        //    }
        //    else if (status.State == AcquisitionStatus.AcquisitionState.NewImage ||
        //             status.TotalImages != Images?.Count)
        //    {
        //        // New image arrived, update Images list (must be called from UI thread)
        //        Application.Current.Dispatcher.Invoke(() =>
        //        {
        //            // Update the list and show latest image
        //            UpdateImageList(status.LastImageId);
        //        });
        //    }
        //}
    }
}
