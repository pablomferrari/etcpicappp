using System.Collections.Generic;
using System.Threading.Tasks;
using EtcPicApp.Models.Captions;
using EtcPicApp.Models.Jobs;
using EtcPicApp.Models.PhotoCaption;

namespace EtcPicApp.Contracts.Services.Data
{
    public interface IApiService
    {
        Task<IEnumerable<Jobs>> GetJobsAsync();
        Task<IEnumerable<Captions>> GetCaptionsAsync();
        Task PostPhotoCaptionsAsync(IEnumerable<PhotoStreamCaption> photos);
        Task PutPhotoCaptionsAsync(IEnumerable<PhotoStreamCaption> photos);
    }
}
