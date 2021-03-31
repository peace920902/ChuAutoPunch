using System.Threading.Tasks;

namespace AutoAttend.Interface
{
    public interface IWifiManager
    {
        Task<bool> ConnectToWifi();
        Task DisConnectWifi();
    }
}