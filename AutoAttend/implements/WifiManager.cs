using System;
using System.Linq;
using System.Threading.Tasks;
using AutoAttend.Interface;
using ManagedNativeWifi;

namespace AutoAttend
{
    public class WifiManager : IWifiManager
    {
        private readonly IErrorHandler _errorHandler;

        public WifiManager(IErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
        }

        public async Task<bool> ConnectToWifi()
        {
            try
            {
                var wireless = NativeWifi.EnumerateAvailableNetworks().FirstOrDefault(x => x.Ssid.ToString().Equals("wireless"));
                if (wireless != null)
                {
                    await NativeWifi.ConnectNetworkAsync(wireless.Interface.Id, wireless.ProfileName, wireless.BssType, timeout: TimeSpan.FromSeconds(30));
                    return true;
                }

                _errorHandler.LogErrorAndDelay("cannot connect wifi");
                if (_errorHandler.CheckError())
                    _errorHandler.HandleError();
                return false;
            }
            catch (Exception e)
            {
                _errorHandler.LogErrorAndDelay(e.Message);
                if (_errorHandler.CheckError()) 
                    _errorHandler.HandleError();
                return false;
            }
        }
    }
}