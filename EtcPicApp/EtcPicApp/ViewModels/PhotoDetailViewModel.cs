using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EtcPicApp.Constants;
using EtcPicApp.Contracts.Services.Data;
using EtcPicApp.Contracts.Services.General;
using EtcPicApp.Extensions;
using EtcPicApp.Models.PhotoCaption;
using EtcPicApp.Models.Sql;
using EtcPicApp.ViewModels.Base;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace EtcPicApp.ViewModels
{
    public class PhotoDetailViewModel : ViewModelBase
    {
        #region Photo Source
        public PhotoStreamCaption _photoCaption;
        public PhotoStreamCaption PhotoCaption
        {
            get => _photoCaption;
            set
            {
                _photoCaption = value;
                OnPropertyChanged();
            }
        }
        public string _photoSource;
        public string PhotoSource
        {
            get => _photoSource;
            set
            {
                _photoSource = value;
                OnPropertyChanged();
            }
        }
        #endregion
        private ObservableCollection<string> _captions;
        public ObservableCollection<string> Captions
        {
            get => _captions;
            set
            {
                _captions = value;
                OnPropertyChanged();
            }
        }

        private readonly ISqlLiteService _dataService;
        private readonly ISettingsService _settingsService;
        public PhotoDetailViewModel(
            IConnectionService connectionService,
            INavigationService navigationService,
            IDialogService dialogService,
            ISqlLiteService dataService, 
            ISettingsService settingsService)
            : base(connectionService, navigationService, dialogService)
        {
            _dataService = dataService;
            _settingsService = settingsService;
            InitializeMessenger();
        }

        private void InitializeMessenger()
        {
        }
        


        public ICommand OnSaveChangesCommand => new Command(SavePhoto);

        public override async Task InitializeAsync(object data)
        {
            IsBusy = true;

            var id = (int) data;
            try
            {
                PhotoCaption = await _dataService.GetPhotoCaptionAsync(id);
                var job = (await _dataService.GetJobsAsync()).FirstOrDefault(x => x.JobId == PhotoCaption.JobId);
                var captionModels = await _dataService.GetCaptionsByServiceAsync(job?.ServiceId ?? 0);
                Captions = captionModels.Select(x => x.Caption).ToObservableCollection();
            }
            catch(Exception ex)
            {
                Crashes.TrackError(ex);
            }
            
            IsBusy = false;
        }


        private async void SavePhoto()
        {
            if(PhotoCaption.Id == 0)
            {
                await _dataService.InsertAsync<PhotoCaptionTable>(PhotoCaption.ToTable());
            }
            else
            {
                await _dataService.UpdateAsync<PhotoCaptionTable>(PhotoCaption.ToTable());
            }

            MessagingCenter.Send(this, MessengerConstants.PhotoAdded);
            await _navigationService.NavigateBackAsync();

        }
    }
}
