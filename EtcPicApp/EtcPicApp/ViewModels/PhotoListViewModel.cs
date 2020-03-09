using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EtcPicApp.Constants;
using EtcPicApp.Contracts.Services.Data;
using EtcPicApp.Contracts.Services.General;
using EtcPicApp.Data;
using EtcPicApp.Extensions;
using EtcPicApp.Models.Jobs;
using EtcPicApp.Models.PhotoCaption;
using EtcPicApp.Models.Sql;
using EtcPicApp.Utility;
using EtcPicApp.ViewModels.Base;
using Plugin.Media;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Xamarin.Forms;

namespace EtcPicApp.ViewModels
{
    public class PhotoListViewModel : ViewModelBase
    {
        private readonly ISqlLiteService _dataService;
        #region props

        public string _photoIcon;
        public string PhotoIcon
        {
            get => _photoIcon;
            set
            {
                _photoIcon = value;
                OnPropertyChanged();
            }
        }
        public string _cameraIcon;
        public string CameraIcon
        {
            get => _cameraIcon;
            set
            {
                _cameraIcon = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<PhotoStreamCaption> _filteredPhotos;
        public ObservableCollection<PhotoStreamCaption> FilteredPhotoCaptions
        {
            get => _filteredPhotos;
            set
            {
                _filteredPhotos = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<PhotoStreamCaption> _photos;
        public ObservableCollection<PhotoStreamCaption> PhotoCaptions
        {
            get => _photos;
            set
            {
                _photos = value;
                OnPropertyChanged();
            }
        }


        private Jobs Job;
        private string _search;
        public string _header;
        public string _address;
        public string _status;
        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }


        public string Search
        {
            get => _search;
            set
            {
                _search = value;
                FilterChanged(value);
                OnPropertyChanged();
            }
        }
        #endregion
        public PhotoListViewModel(IConnectionService connectionService,
            INavigationService navigationService,
            IDialogService dialogService,
            ISqlLiteService dataService)
            : base(connectionService, navigationService, dialogService)
        {
            _dataService = dataService;
            InitializeMessenger();
        }

        public void InitializeMessenger()
        {
            PhotoIcon = Icons.PhotoLibrary;
            CameraIcon = Icons.PhotoCamera;
            MessagingCenter.Subscribe<PhotoDetailViewModel>(this,
                MessengerConstants.PhotoAdded,
                async (sender) => await LoadPhotos());
        }

        public ICommand PhotoCaptionTappedCommand => new Command<PhotoStreamCaption>(OnPhotoCaptionTapped);
        public ICommand AddFromCameraCommand => new Command(AddPhotoCommand);
        public ICommand GetFromGalleryCommand => new Command(GetPhotoCommand);
        public ICommand ReportCommand => new Command(GenerateReport);

        private void GenerateReport(object obj)
        {
            // Create a new PDF document
            PdfDocument document = new PdfDocument();

            //Add a page to the document
            PdfPage page = document.Pages.Add();

            //Create PDF graphics for the page
            PdfGraphics graphics = page.Graphics;

            //Set the standard font
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

            //Draw the text
            graphics.DrawString("Hello World!!!", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 0));

            //Save the document to the stream
            MemoryStream stream = new MemoryStream();
            document.Save(stream);

            //Close the document
            document.Close(true);

            //Save the stream as a file in the device and invoke it for viewing
            Xamarin.Forms.DependencyService.Get<IFileSave>().SaveAndView("Output.pdf", "application / pdf", stream);
        }

        public ICommand DeleteCommand => new Command(async (data) =>
        {
            await DeletePhotoCaption((PhotoStreamCaption)data);
            await LoadPhotos();
        });

        public ICommand ViewPhotoCaptionCommand => new Command<PhotoStreamCaption>(OnViewPhotoCaption);

        public ICommand DoneCommand => new Command(async () => { await _navigationService.NavigateBackAsync(); });

        private async void OnViewPhotoCaption(PhotoStreamCaption data)
        {
            await _navigationService.NavigateToAsync<PhotoDetailViewModel>(data.Id);
        }


        private async Task DeletePhotoCaption(PhotoStreamCaption data)
        {
            var result = await _dialogService.ShowConfirm(
                "Are you sure you want to delete this photo?",
                "Delete Photo/Caption",
                "Yes", "Cancel");
            if (!result) return;
            await _dataService.DeleteAsync<PhotoCaptionTable>(data.Id);
        }


        private async void AddPhotoCommand()
        {
            try
            {
                await CrossMedia.Current.Initialize();

                var status = CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported;
                var photo = await CrossMedia.Current.TakePhotoAsync(
                    new Plugin.Media.Abstractions.StoreCameraMediaOptions() { 
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium });

                if (photo != null)
                {
                    await AddPhoto(photo.Path);
                }
              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task AddPhoto(string path)
        {
            var result = await _dataService.InsertAsync<PhotoCaptionTable>(new PhotoCaptionTable
            {
                Caption = "",
                JobId = Job.JobId,
                Path = path
            });

            await _navigationService.NavigateToAsync<PhotoDetailViewModel>(result);
        }

        private async void GetPhotoCommand()
        {
            try
            {
                await CrossMedia.Current.Initialize();

                var status = CrossMedia.Current.IsPickPhotoSupported;
                var photo = await CrossMedia.Current.PickPhotoAsync(
                    new Plugin.Media.Abstractions.PickMediaOptions() { 
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                    });

                if (photo != null)
                {
                    await AddPhoto(photo.Path);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void OnPhotoCaptionTapped(PhotoStreamCaption data)
        {
            if (data == null) return;
            await _navigationService.NavigateToAsync<PhotoDetailViewModel>(data.Id);
        }

        #region loading
        private void FilterChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                FilteredPhotoCaptions = PhotoCaptions.ToObservableCollection();
            }
            else
            {
                FilteredPhotoCaptions
                    = PhotoCaptions.Where(x => x.Caption.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToObservableCollection();
            }
        }
        public override async Task InitializeAsync(object data)
        {
            IsBusy = true;
            Job = (Jobs)data;
            Header = string.Concat(Job.JobId, " ", Job.Client?.Name);
            Address = Job.FacilityAddress;
            await LoadPhotos(false);
            IsBusy = false;
        }
        private async Task LoadPhotos(bool forceRefresh = false)
        {
            PhotoCaptions = (await _dataService.GetPhotoStreamCaptionsAsync(Job.JobId)).ToObservableCollection();
            FilteredPhotoCaptions = PhotoCaptions ?? new ObservableCollection<PhotoStreamCaption>();
            FilterChanged(_search);
        }
        #endregion
    }
}