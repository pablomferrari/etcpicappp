using System.Collections.Generic;
using System.Threading.Tasks;
using EtcPicApp.Models.General;
using EtcPicApp.Models.Jobs;
using EtcPicApp.Models.Materials;
using EtcPicApp.Models.Samples;

namespace EtcPicApp.Contracts.Services.Data
{
    public interface IDataService
    {
        Task<IEnumerable<Jobs>> EtcJobs();
        Task<IEnumerable<Materials>> Materials(int jobId, bool force = false);
        Task<IEnumerable<Samples>> Samples(int materialId, bool force = false);
        Task UpdateMaterial(Materials material);
        Task InsertMaterial(Materials material);
        Task UpdateSample(Samples sample);
        Task InsertSample(Samples sample);

        Task PostMaterials();
        Task PutMaterials();
        Task PostSamples();
        Task PutSamples();
        Task ClearCache();
        Task DeliverSamples(DeliveryRequest deliveryRequest);
        Task PostDeliveries();

        void AddAll<T>(T values, string key);

        Task<IEnumerable<Materials>> GetMaterialsAsync();
        Task<IEnumerable<Samples>> GetSamplesAsync();
        IEnumerable<string> GetKeys();
        Task<SurveyData> GetSurveyDataAsync(string key);
    }
}
