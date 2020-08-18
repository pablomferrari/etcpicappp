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
using EtcPicApp.Models.Jobs;
using EtcPicApp.Models.PhotoCaption;
using EtcPicApp.Services.Data;
using EtcPicApp.ViewModels.Base;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using Xamarin.Forms;

namespace EtcPicApp.ViewModels
{
    public class JobListViewModel : ViewModelBase
    {
        private readonly ISqlLiteService _dataService;
        private readonly ISettingsService _settingsService;
        private readonly ISynchronizationService _synchronizationService;

        private ObservableCollection<Jobs> _jobs;
        private ObservableCollection<Jobs> _filteredJobs;
        public ObservableCollection<Jobs> FilteredJobs
        {
            get => _filteredJobs;
            set
            {
                _filteredJobs = value;
                OnPropertyChanged();
            }
        }
        private string _search;
        public JobListViewModel(
            IConnectionService connectionService, 
            INavigationService navigationService, 
            IDialogService dialogService,
            ISqlLiteService dataService,
            ISettingsService settings, 
            ISynchronizationService synchronizationService) 
            : base(connectionService, navigationService, dialogService)
        {
            _dataService = dataService;
            _settingsService = settings;
           _synchronizationService = synchronizationService;
            InitializeMessenger();
        }

        public void InitializeMessenger()
        {
            MessagingCenter.Subscribe<DataService, string>(this,
                MessengerConstants.DataServiceError,
                (sender, value) => OnDataServiceError(value));

            MessagingCenter.Subscribe<App>(this,
                MessengerConstants.DataServiceError,
                (sender) => RefreshJobs());
        }

        private async void OnDataServiceError(string value)
        {
            await _dialogService.ShowDialog(
                "Please get a screen-shot of the following. Something went wrong: " + value,
                "Error",
                "OK");
        }

        private async void OnPhotoAdded()
        {
            await LoadJobs();
        }

        public ICommand JobTappedCommand => new Command<Jobs>(OnJobTapped);
        public ICommand SubmitCommand => new Command(Submit);

        
        public ICommand LogOutCommand => new Command(LogOut);

        private void LogOut()
        {
            _navigationService.NavigateToAsync<LoginViewModel>();
        }

        public ICommand RefreshCommand => new Command(ReloadJobs);

        private async void Submit(object obj)
        {
            try
            {
                var isReachable = await _connectionService.IsApiReachable();
                if (!isReachable)
                {
                    await _dialogService.ShowDialog(
                        "Either you are not connected to internet or server is down at this moment. Please try again later.",
                         "Error",
                         "OK");
                    return;
                }
                IsBusy = true;
                await _synchronizationService.SynchronizeData();
                await LoadJobs();
                IsBusy = false;
                await _dialogService.ShowDialog(
                    "Jobs and captions have been updated",
                    "Update",
                    "OK");

                Analytics.TrackEvent("Data Updated", new Dictionary<string, string> {
                    { _settingsService.UserNameSetting, DateTime.Now.ToString("G") }
                });
            }
            catch (Exception e)
            {
                await _dialogService.ShowDialog(
                    $"Something went wrong: {e.Message}",
                    "Error",
                    "OK");
                Crashes.TrackError(e);
                IsBusy = false;

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

        private async void OnJobTapped(Jobs selectedJob)
        {
            await _navigationService.NavigateToAsync<PhotoListViewModel>(selectedJob);
        }

        public override async Task InitializeAsync(object data)
        {
            IsBusy = true;
            // await IsFirstTime();
            await LoadJobs();
            IsBusy = false;
        }

        private async Task IsFirstTime()
        {
            try
            {
                var jobsRan = _settingsService.GetItem(Constants.DatabaseConstants.FirstTimeFlag);
                if (string.IsNullOrEmpty(jobsRan))
                {
                    await _synchronizationService.SynchronizeData();
                    _settingsService.AddItem(DatabaseConstants.FirstTimeFlag, "set");
                }
            }
            catch (Exception e)
            {
                MessagingCenter.Send(this, "Sync errors", "Errors synchronizing data: " + e.Message);
            }
        }

        private void RefreshJobs()
        {
            Task.Run(async () => await LoadJobs());
        }

        private async Task LoadJobs()
        {
            var result = await _dataService.GetJobsAsync();
            _jobs = result.ToObservableCollection();
            FilteredJobs = _jobs;
            FilterChanged(Search);
        }

        private async void ReloadJobs()
        {
            IsBusy = true;
            if (_connectionService.IsConnected)
            {
                await LoadJobs();
            }
            else
            {
                _dialogService.ShowToast("You are not connected to internet!");
            }
            IsBusy = false;
        }


        private void FilterChanged(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                    FilteredJobs = _jobs;
                else
                {
                    FilteredJobs = _jobs.Where(x => x.JobId.ToString().Contains(value)
                                                    ||
                                                        
                                                        !string.IsNullOrEmpty(x.FacilityAddress)
                                                            && x.FacilityAddress.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0
                                                    ).ToObservableCollection();
                }
            }
            catch (Exception e)
            {
                Search = string.Empty;
                Crashes.TrackError(e);
                
            }
        }
    }
}
