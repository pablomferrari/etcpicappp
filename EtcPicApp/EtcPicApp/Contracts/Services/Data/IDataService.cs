using System.Collections.Generic;
using System.Threading.Tasks;
using EtcPicApp.Models.Captions;
using EtcPicApp.Models.Jobs;
using EtcPicApp.Models.PhotoCaption;

namespace EtcPicApp.Contracts.Services.Data
{
    public interface IDataService
    {
        Task<IEnumerable<Jobs>> EtcJobsAsync();
        Task<IEnumerable<PhotoCaption>> GetPhotoCaptionsAsync(int jobId);
        Task<IEnumerable<Captions>> GetCaptionsAsync();
        Task UpdatePhotoCaptionAsync(PhotoCaption photoCaption);
        Task InsertPhotoCaptionAsync(PhotoCaption photoCaption);
        Task PostPhotoStreamCaptionAsync(ICollection<PhotoStreamCaption> photoStreamCaptions);
        Task PutPhotoStreamCaptionAsync(ICollection<PhotoStreamCaption> photoStreamCaptions);
        Task ClearCache();
        void AddAll<T>(T values, string key);
        IEnumerable<string> GetKeys();
    }
}
