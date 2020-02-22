using System.Threading.Tasks;
using Plugin.Connectivity.Abstractions;

namespace EtcPicApp.Contracts.Services.General
{
    public interface IConnectionService
    {
        bool IsConnected { get; }
        Task<bool> IsApiReachable();
        event ConnectivityChangedEventHandler ConnectivityChanged;
    }
}
