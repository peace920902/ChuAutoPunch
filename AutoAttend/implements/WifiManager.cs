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
        private readonly ISeleniumManipulator _seleniumManipulator;

        public WifiManager(IErrorHandler errorHandler, ISeleniumManipulator seleniumManipulator)
        {
            _errorHandler = errorHandler;
            _seleniumManipulator = seleniumManipulator;
        }

        public async Task<bool> ConnectToWifi()
        {
            try
            {
                var wireless = NativeWifi.EnumerateAvailableNetworks().FirstOrDefault(x => x.Ssid.ToString().Equals("wireless"));
                if (wireless != null)
                {
                    await NativeWifi.ConnectNetworkAsync(wireless.Interface.Id, wireless.ProfileName, wireless.BssType, TimeSpan.FromSeconds(30));
                    _seleniumManipulator.LoginWifi();
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

        public async Task DisConnectWifi()
        {
            var wireless = NativeWifi.EnumerateAvailableNetworks().FirstOrDefault(x => x.Ssid.ToString().Equals("wireless"));
            if (wireless != null)
            {
                await NativeWifi.DisconnectNetworkAsync(wireless.Interface.Id, TimeSpan.FromSeconds(30));
            }
        }
    }
}