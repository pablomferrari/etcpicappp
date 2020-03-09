using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
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
    public class DataService : BaseService, IDataService
    {
        private readonly IGenericRepository _genericRepository;

        public DataService(IGenericRepository genericRepository,
            IBlobCache cache = null) : base(cache)
        {
            _genericRepository = genericRepository;
        }


       public async Task<IEnumerable<Jobs>> EtcJobsAsync()
        {
            var jobsFromCache =
                  await GetFromCache<List<Jobs>>(CacheNameConstants.EtcJobs);

            if (jobsFromCache != null)
            {
                return jobsFromCache;
            }
            var builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.EtcJobs
            };
            var jobs = await _genericRepository.GetAsync<List<Jobs>>(builder.ToString());
            try
            {
                await Cache.InsertObject(CacheNameConstants.EtcJobs, jobs);
            }
            catch (Exception e)
            {
                MessagingCenter.Send(this, MessengerConstants.DataServiceError, "Error retrieving jobs: " + e.Message);
                Crashes.TrackError(e);
                throw;
            }
            return jobs;
        }

       public Task<IEnumerable<PhotoCaption>> GetPhotoCaptionsAsync(int jobId)
       {
           throw new NotImplementedException();
       }

       public Task<IEnumerable<Captions>> GetCaptionsAsync()
       {
           throw new NotImplementedException();
       }

       public Task UpdatePhotoCaptionAsync(PhotoCaption photoCaption)
       {
           throw new NotImplementedException();
       }

       public Task InsertPhotoCaptionAsync(PhotoCaption photoCaption)
       {
           throw new NotImplementedException();
       }

       public Task PostPhotoStreamCaptionAsync(ICollection<PhotoStreamCaption> photoStreamCaptions)
       {
           throw new NotImplementedException();
       }

       public Task PutPhotoStreamCaptionAsync(ICollection<PhotoStreamCaption> photoStreamCaptions)
       {
           throw new NotImplementedException();
       }


        //public async Task UpdateMaterial(Materials material)
        // {
        //     try
        //     {
        //         var materials = await GetFromCache<List<Materials>>(CacheNameConstants.Materials);
        //         var updatedMaterial = materials.FirstOrDefault(x =>
        //             x.JobId == material.JobId && x.ClientMaterialId == material.ClientMaterialId);
        //         var index = materials.IndexOf(updatedMaterial);
        //         if (index > 0)
        //         {
        //             materials[index] = material;
        //             await Cache.InsertObject(CacheNameConstants.Materials, materials);
        //         }

        //     }
        //     catch (Exception e)
        //     {
        //         MessagingCenter.Send(this, MessengerConstants.DataServiceError, "Error updating material: " + e.Message);
        //         Crashes.TrackError(e);
        //     }

        // }

        // public async Task InsertMaterial(Materials material)
        // {
        //     material.CanViewSamples = true;
        //     var materials = await GetFromCache<List<Materials>>(CacheNameConstants.Materials);
        //     materials.Add(material);
        //     await Cache.InsertObject(CacheNameConstants.Materials, materials);
        // }

        // public async Task UpdateSample(Samples sample)
        // {
        //     try
        //     {
        //         var samples = await GetFromCache<List<Samples>>(CacheNameConstants.Samples);
        //         var updatedSample = samples.FirstOrDefault(x =>
        //             x.JobId == sample.JobId && x.ClientSampleId == sample.ClientSampleId);
        //         var index = samples.IndexOf(updatedSample);
        //         if (index > 0)
        //         {
        //             samples[index] = sample;
        //             await Cache.InsertObject(CacheNameConstants.Samples, samples);
        //         }
        //     }
        //     catch (Exception e)
        //     {
        //         MessagingCenter.Send(this, MessengerConstants.DataServiceError, "Error updating sample: " + e.Message);
        //         Crashes.TrackError(e);
        //     }
        // }

        // public async Task InsertSample(Samples sample)
        // {
        //     var samples = await GetFromCache<List<Samples>>(CacheNameConstants.Samples);
        //     samples.Add(sample);
        //     await Cache.InsertObject(CacheNameConstants.Samples, samples);
        // }

        // public async Task PostMaterials()
        // {
        //     var materials = await GetFromCache<List<Materials>>(CacheNameConstants.Materials);
        //     var newMaterials = materials.Where(x => x.IsLocal && x.Id < 0).ToList();

        //     if (!newMaterials.Any(x => x.IsLocal))//loaded from cache
        //     {
        //         return;
        //     }
        //     var builder = new UriBuilder(ApiConstants.BaseApiUrl)
        //     {
        //         Path = ApiConstants.PostMaterials
        //     };
        //     try
        //     {
        //         await _genericRepository.PostAsync(builder.ToString(), newMaterials);
        //     }
        //     catch (Exception e)
        //     {
        //         MessagingCenter.Send(this, MessengerConstants.DataServiceError, "Error posting materials: " + e.Message);
        //         Crashes.TrackError(e);
        //         throw;
        //     }
        // }

        // public async Task PutMaterials()
        // {
        //     var materialsFromCache =
        //         await GetFromCache<List<Materials>>(CacheNameConstants.Materials);


        //     if (materialsFromCache == null || !materialsFromCache.Any(x => x.IsLocal))//loaded from cache
        //     {
        //         return;
        //     }

        //     // var putValues = materialsFromCache.Where(x => x.IsLocal).ToList();
        //     var putValues = materialsFromCache.Where(x => x.IsLocal && x.Id > 0).ToList();
        //     if (!putValues.Any()) return;
        //     var builder = new UriBuilder(ApiConstants.BaseApiUrl)
        //     {
        //         Path = ApiConstants.PutMaterials
        //     };
        //     try
        //     {
        //         await _genericRepository.PutAsync(builder.ToString(), putValues);
        //     }
        //     catch (Exception e)
        //     {
        //         MessagingCenter.Send(this, MessengerConstants.DataServiceError, "Error updating materials: " + e.Message);
        //         Crashes.TrackError(e);
        //     }

        // }

        // public async Task PostSamples()
        // {
        //     var samplesFromCache =
        //         await GetFromCache<List<Materials>>(CacheNameConstants.Samples);

        //     if (samplesFromCache == null || !samplesFromCache.Any(x => x.IsLocal))//loaded from cache
        //     {
        //         return;
        //     }

        //     var postValues = samplesFromCache.Where(x => x.Id < 0).ToList();
        //     if (!postValues.Any()) return;
        //     var builder = new UriBuilder(ApiConstants.BaseApiUrl)
        //     {
        //         Path = ApiConstants.PostSamples
        //     };
        //     try
        //     {
        //         await _genericRepository.PostAsync(builder.ToString(), postValues);
        //     }
        //     catch (Exception e)
        //     {
        //         MessagingCenter.Send(this, MessengerConstants.DataServiceError, "Error posting samples: " + e.Message);
        //         Crashes.TrackError(e);
        //     }
        // }

        // public async Task PutSamples()
        // {
        //     var samplesFromCache =
        //         await GetFromCache<List<Materials>>(CacheNameConstants.Samples);


        //     if (samplesFromCache == null || !samplesFromCache.Any(x => x.IsLocal))//loaded from cache
        //     {
        //         return;
        //     }

        //     var putValues = samplesFromCache.Where(x => x.IsLocal && x.Id > 0).ToList();
        //     if (!putValues.Any()) return;
        //     var builder = new UriBuilder(ApiConstants.BaseApiUrl)
        //     {
        //         Path = ApiConstants.PutMaterials
        //     };
        //     try
        //     {
        //         await _genericRepository.PutAsync(builder.ToString(), putValues);
        //     }
        //     catch (Exception e)
        //     {
        //         MessagingCenter.Send(this, MessengerConstants.DataServiceError, "Error updating samples: " + e.Message);
        //         Crashes.TrackError(e);
        //     }
        // }


        // public async Task DeliverSamples(DeliveryRequest deliveryRequest)
        // {
        //     var deliveries = await GetFromCache<List<DeliveryRequest>>(CacheNameConstants.DeliveryRequests) 
        //         ?? new List<DeliveryRequest>();

        //     if (deliveries.All(x => x.JobId != deliveryRequest.JobId))
        //     {
        //         deliveries.Add(deliveryRequest);
        //         await Cache.InsertObject(CacheNameConstants.DeliveryRequests, deliveries);
        //     }
        //     else
        //     {
        //         var updatedDelivery = deliveries.FirstOrDefault(x => x.JobId == deliveryRequest.JobId);
        //         var index = deliveries.IndexOf(updatedDelivery);
        //         deliveries[index] = deliveryRequest;
        //         await Cache.InsertObject(CacheNameConstants.Samples, deliveries);
        //     }

        // }

        // public async Task PostDeliveries()
        // {
        //     var deliveriesFromCache =
        //         await GetFromCache<List<DeliveryRequest>>(CacheNameConstants.DeliveryRequests);

        //     if (deliveriesFromCache == null)//loaded from cache
        //     {
        //         return;
        //     }

        //     var builder = new UriBuilder(ApiConstants.BaseApiUrl)
        //     {
        //         Path = ApiConstants.PostDeliveries
        //     };
        //     try
        //     {
        //         await _genericRepository.PostAsync(builder.ToString(), deliveriesFromCache);
        //     }
        //     catch (Exception e)
        //     {
        //         MessagingCenter.Send(this, MessengerConstants.DataServiceError, "Error delivering samples: " + e.Message);
        //         Crashes.TrackError(e);
        //     }
        // }


        // public async Task<IEnumerable<Materials>> GetMaterialsAsync()
        // {
        //     return await GetFromCache<List<Materials>>(CacheNameConstants.NewMaterials);
        // }

        // public async Task<IEnumerable<Samples>> GetSamplesAsync()
        // {
        //     return await GetFromCache<List<Samples>>(CacheNameConstants.NewSamples);
        // }
        // public async Task<SurveyData> GetSurveyDataAsync(string key)
        // {
        //     return await GetFromCache<SurveyData>("ETCSyncData_" + key);
        // }

        public IEnumerable<string> GetKeys()
        {
            var allKeys = Cache.GetAllKeys().Select(x => x).Wait();
            var keys = new List<string>(allKeys.Where(x => x.StartsWith("ETCSyncData_")));
            return keys.Select(x => x.Replace("ETCSyncData_", string.Empty)).OrderByDescending(x => x);
        }

        public void AddAll<T>(T values, string key)
        {
            Cache.InsertObject(key, values, TimeSpan.FromDays(15));
        }
        public async Task ClearCache()
        {
            await BlobCache.LocalMachine.InvalidateAll();
            await BlobCache.LocalMachine.Vacuum();
        }

    }
}
