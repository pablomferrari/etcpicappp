using System.Threading.Tasks;

namespace EtcPicApp.Contracts.Services.Data
{
    public interface ISynchronizationService
    {
        Task SynchronizeData();
        Task SyncCaptionsAsync();
    }
}
