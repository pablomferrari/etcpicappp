using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EtcPicApp.Constants;
using EtcPicApp.Contracts.Repository;
using EtcPicApp.Contracts.Services.Data;
using EtcPicApp.Models.Captions;
using EtcPicApp.Models.Jobs;
using EtcPicApp.Models.PhotoCaption;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace EtcPicApp.Services.Data
{
    public class ApiService : IApiService
    {
        private readonly IGenericRepository _genericRepository;
        public ApiService(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<IEnumerable<Jobs>> GetJobsAsync()
        {
            var builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.Jobs
            };
            return await _genericRepository.GetAsync<List<Jobs>>(builder.ToString());
        }

        public async Task<IEnumerable<Captions>> GetCaptionsAsync()
        {
            var builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.Captions
            };
            return await _genericRepository.GetAsync<List<Captions>>(builder.ToString());
        }

        public async Task PostPhotoCaptionsAsync(IEnumerable<PhotoStreamCaption> photos)
        {
            var list = photos.ToList();
            if (!list.Any()) return;
            var builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.PostPhotoStreamCaptions
            };
            try
            {
                await _genericRepository.PostAsync(builder.ToString(), list);
            }
            catch (Exception e)
            {
                MessagingCenter.Send(this, MessengerConstants.DataServiceError, "Error posting images with captions: " + e.Message);
                Crashes.TrackError(e);
            }
        }

        public async Task PutPhotoCaptionsAsync(IEnumerable<PhotoStreamCaption> photos)
        {
            var list = photos.ToList();
            if (!list.Any()) return;
            var builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.PutPhotoStreamCaptions
            };
            try
            {
                await _genericRepository.PutAsync(builder.ToString(), list);
            }
            catch (Exception e)
            {
                MessagingCenter.Send(this, MessengerConstants.DataServiceError, "Error updating images and captions: " + e.Message);
                Crashes.TrackError(e);
                throw;
            }
        }
    }
}
