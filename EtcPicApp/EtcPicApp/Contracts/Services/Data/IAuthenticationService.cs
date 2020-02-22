using System.Threading.Tasks;
using EtcPicApp.Models.Users;

namespace EtcPicApp.Contracts.Services.Data
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> Authenticate(string userName, string password);

        bool IsUserAuthenticated();
    }
}
