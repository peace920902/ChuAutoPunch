using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoAttend.Interface;
using ManagedNativeWifi;
using Microsoft.Extensions.Logging;

namespace AutoAttend
{
    public class WifiManager : IWifiManager
    {
        private readonly IErrorHandler _errorHandler;
        private readonly ISeleniumManipulator _seleniumManipulator;
        private readonly ILogger<WifiManager> _logger;

        public WifiManager(IErrorHandler errorHandler, ISeleniumManipulator seleniumManipulator, ILogger<WifiManager> logger)
        {
            _errorHandler = errorHandler;
            _seleniumManipulator = seleniumManipulator;
            _logger = logger;
        }

        public async Task<bool> ConnectToWifi()
        {
            try
            {
                var wireless = NativeWifi.EnumerateAvailableNetworks().FirstOrDefault(x => string.Equals(x.Ssid.ToString(), "wireless", StringComparison.CurrentCultureIgnoreCase));
                if (wireless != null)
                {
                    _logger.Log(LogLevel.Information, JsonSerializer.Serialize(wireless));
                    await NativeWifi.ConnectNetworkAsync(wireless.Interface.Id, wireless.ProfileName ?? "wireless", wireless.BssType, TimeSpan.FromSeconds(30));
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