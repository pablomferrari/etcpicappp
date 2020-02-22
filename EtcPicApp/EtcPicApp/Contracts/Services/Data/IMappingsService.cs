using System.Collections.Generic;
using System.Threading.Tasks;
using EtcPicApp.Models.Materials;

namespace EtcPicApp.Contracts.Services.Data
{
    public interface IMappingsService
    {
        Task<IEnumerable<Mapping>> GetMappings(bool force = false);
    }
}
