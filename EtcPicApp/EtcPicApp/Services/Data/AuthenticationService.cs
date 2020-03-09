using System;
using System.Threading.Tasks;
using EtcPicApp.Constants;
using EtcPicApp.Contracts.Repository;
using EtcPicApp.Contracts.Services.Data;
using EtcPicApp.Contracts.Services.General;
using EtcPicApp.Models.Users;

namespace EtcPicApp.Services.Data
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IGenericRepository _genericRepository;
        private readonly ISettingsService _settingsService;
        public AuthenticationService(IGenericRepository genericRepository, ISettingsService settingsService)
        {
            _settingsService = settingsService;
            _genericRepository = genericRepository;

        }


        public bool IsUserAuthenticated()
        {
            return !string.IsNullOrEmpty(_settingsService.UserIdSetting);
        }

        public async Task<AuthenticationResponse> Authenticate(string userName, string password)
        {
#if DEBUG
            return new AuthenticationResponse
            {
                IsAuthenticated = true,
                Employee = new Employee
                {
                    Name = "Test Employee"
                }
            };
#endif
            UriBuilder builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.AuthenticateEtcLogin
            };

            AuthenticationRequest authenticationRequest = new AuthenticationRequest()
            {
                UserName = userName,
                Password = password
            };

            return await _genericRepository.PostAsync<AuthenticationRequest, AuthenticationResponse>(builder.ToString(), authenticationRequest);
        }
    }
}
