using System.Collections.Generic;
using System.Threading.Tasks;
using EtcPicApp.Models.Captions;
using EtcPicApp.Models.Jobs;
using EtcPicApp.Models.PhotoCaption;
using EtcPicApp.Models.Sql;

namespace EtcPicApp.Contracts.Services.Data
{
    public interface ISqlLiteService
    {
        Task<IEnumerable<Jobs>> GetJobsAsync();
        Task<int> UpdateAsync<T>(T value) where T : BaseTable;
        Task<int> InsertAsync<T>(T value) where T : BaseTable;
        Task<int> InsertOrUpdateAllAsync<T>(List<T> value) where T : BaseTable;
        Task<int> InsertAllAsync<T>(List<T> value);
        Task<IEnumerable<PhotoCaption>> GetAllPhotoCaptionsAsync();
        Task<IEnumerable<PhotoCaption>> GetPhotoCaptionsAsync(int jobId);
        Task<IEnumerable<PhotoStreamCaption>> GetPhotoStreamCaptionsAsync(int jobId);
        Task<IEnumerable<Captions>> GetAllCaptionsAsync();
        Task<IEnumerable<Captions>> GetCaptionsByServiceAsync(int serviceId);
        Task<int> DeleteAsync<T>(int id);
        Task<IEnumerable<PhotoStreamCaption>> GetPostPhotoStreamCaptions();
        Task<IEnumerable<PhotoStreamCaption>> GetPutPhotoStreamCaptions();
        void DropAndRecreateJobs();
        void DropAndRecreateCaptions();
        Task<PhotoStreamCaption> GetPhotoCaptionAsync(int id);
    }
}
