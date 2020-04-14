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
using Microsoft.AppCenter.Crashes;
using Plugin.Media;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Tables;
using Xamarin.Forms;

namespace EtcPicApp.ViewModels
{
    public class PhotoListViewModel : ViewModelBase
    {
        private readonly ISqlLiteService _dataService;
        #region props

        public string _requiredCaptions;

        public string RequiredCaptions
        {
            get => _requiredCaptions;
            set
            {
                _requiredCaptions = value;
                OnPropertyChanged();
            }
        }

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

        public ICommand ViewCaptionsCommand => new Command(ViewCaptions);

        private async void ViewCaptions(object obj)
        {
            var data = await _dataService.GetCaptionsByServiceAsync(Job?.ServiceId ?? 0);
            var text = data.OrderBy(x => x.Caption)
                .Select(x => $"{x.Caption} {(x.Required ? "(Required)" : "")}").ToList();
            var final = string.Join(Environment.NewLine, text);
            await _dialogService.ShowDialog(
                final,
                "Captions",
                "Got it");
        }


        private Stream[] streams = new Stream[0];
        private string[] content = new string[0];

        private async void GenerateReport(object obj)
        {
            IsBusy = true;
            var images = await _dataService.GetPhotoStreamCaptionsAsync(Job.JobId);
            
            // Create a new PDF document
            PdfDocument document = new PdfDocument();

            foreach(var batch in images.Batch(4))
            {
                try
                {
                    streams = batch.Select(x => x.Stream).ToArray();
                    content = batch.Select(x => x.Caption).ToArray();
                    var page = document.Pages.Add();
                    var pdfGrid = new PdfLightTable();
                   
                    pdfGrid.DataSourceType = PdfLightTableDataSourceType.TableDirect;
                    pdfGrid.Columns.Add(new PdfColumn());
                    pdfGrid.Columns.Add(new PdfColumn());

                    pdfGrid.Rows.Add(new object[] { "", "" });
                    pdfGrid.Rows.Add(new object[] { "", "" });
                    pdfGrid.Rows.Add(new object[] { "", "" });
                    pdfGrid.Rows.Add(new object[] { "", "" });
                    pdfGrid.Style.CellPadding = 5;
                    pdfGrid.Style.BorderPen = new PdfPen(new PdfColor(Syncfusion.Drawing.Color.Transparent));
                    var defaultStyle = new PdfCellStyle
                    {
                        TextBrush = PdfBrushes.Black,
                        BorderPen = new PdfPen(new PdfColor(Syncfusion.Drawing.Color.Transparent))
                    };
                    pdfGrid.Style.DefaultStyle = defaultStyle;
                    pdfGrid.BeginCellLayout += pdfLightTable_BeginCellLayout;
                    pdfGrid.BeginRowLayout += pdfLightTable_BeginRowLayout;
                    pdfGrid.Style.CellSpacing = 10;
                    

                    pdfGrid.Draw(page, new Syncfusion.Drawing.RectangleF(0, 50, page.GetClientSize().Width, page.GetClientSize().Height));
                }
                catch(Exception ex)
                {
                    Crashes.TrackError(ex);
                }


            }



            //Save the document to the stream
            MemoryStream stream = new MemoryStream();
            document.Save(stream);

            //Close the document
            document.Close(true);

            //Save the stream as a file in the device and invoke it for viewing
            await DependencyService.Get<IFileSave>().SaveAndView("Report.pdf", "application/pdf", stream);
            IsBusy = false;
        }


        private void pdfLightTable_BeginRowLayout(object sender, BeginRowLayoutEventArgs args)
        {
            if(args.RowIndex % 2 == 0)
            {
                args.MinimalHeight = 300;
            }else
            {
                args.MinimalHeight = 35;
            }
            
        }

        private void pdfLightTable_BeginCellLayout(object sender, BeginCellLayoutEventArgs args)
        {
            try
            {
                if ((args.RowIndex % 2) == 0)
                {
                    var position = args.RowIndex + args.CellIndex;
                    var stream = position < streams.Length ? streams[position] : null;
                    var image = PdfImage.FromStream(stream);
                    args.Graphics.DrawImage(image, args.Bounds);
                }
                else
                {
                    var position = args.RowIndex - 1 + args.CellIndex;
                    var font = new PdfStandardFont(PdfFontFamily.Helvetica, 14);
                    var format = new PdfStringFormat();
                    format.Alignment = PdfTextAlignment.Center;
                    var text = position < content.Length ? content[position] : string.Empty;
                    args.Graphics.DrawString(text, font, PdfBrushes.Black, args.Bounds, format);
                }   

            }
            catch(Exception ex)
            {
                Crashes.TrackError(ex);
            }

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
            await _navigationService.NavigateToAsync<PhotoDetailViewModel>(data);
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
            var pc = await _dataService.GetAllCaptionsAsync();
            var captionCount = pc.Count(x => x.ServiceId == Job.ServiceId && x.Required);
            RequiredCaptions = $"Required pictures: {captionCount}";
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