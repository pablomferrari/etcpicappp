using System;
using System.Linq;
using System.Threading.Tasks;
using EtcPicApp.Contracts.Services.Data;
using EtcPicApp.Contracts.Services.General;
using EtcPicApp.Extensions;
using EtcPicApp.Models.Captions;
using EtcPicApp.Models.Jobs;
using EtcPicApp.Models.Sql;
using Microsoft.AppCenter.Crashes;

namespace EtcPicApp.Services.Data
{
    public class SynchronizationService : ISynchronizationService
    {
        private readonly ISqlLiteService _dataService;
        private readonly IApiService _apiService;
        private readonly ISettingsService _settingsService;
        private readonly IDataService _cacheService;

        public SynchronizationService(
            ISqlLiteService dataService, 
            IApiService apiService, 
            ISettingsService settingsService, 
            IDataService cacheService)
        {
            _dataService = dataService;
            _apiService = apiService;
            _settingsService = settingsService;
            _cacheService = cacheService;
        }

        public async Task SynchronizeData()
        {

            
            try
            {
                //var postPictureStreamCaptions = (await _dataService.GetPostPhotoStreamCaptions()).ToList();
                //var putPictureStreamCaptions = (await _dataService.GetPutPhotoStreamCaptions()).ToList();

                //await _apiService.PostPhotoCaptionsAsync(postPictureStreamCaptions);
                //await _apiService.PutPhotoCaptionsAsync(putPictureStreamCaptions);
                await SyncJobsAsync();
                await SyncCaptionsAsync();
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                throw;
            }
        }


        public async Task SyncJobsAsync()
        {
            try
            {
                var jobs = await _apiService.GetJobsAsync();
                var jobTableList = jobs.Select(Jobs.ToJobsTable).ToList();
                _dataService.DropAndRecreateJobs();
                await _dataService.InsertAllAsync<JobsTable>(jobTableList);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                throw;
            }
        }

        public async Task SyncCaptionsAsync()
        {
            try
            {
                var captions = await _apiService.GetCaptionsAsync();
                _dataService.DropAndRecreateCaptions();
                var captionsList = captions as Captions[] ?? captions.ToArray();
                var captionsTable = captionsList.ToTable();
                var added = await _dataService.InsertAllAsync<CaptionsTable>(captionsTable.ToList());
                _settingsService.AddItem(Constants.DatabaseConstants.Captions_Refreshed, added.ToString());
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                throw;
            }
        }
    }
}
