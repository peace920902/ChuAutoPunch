using System.Threading.Tasks;
using AutoAttend.Interface;
using Microsoft.Extensions.Configuration;

namespace AutoAttend
{
    public class Worker
    {
        private readonly IErrorHandler _errorHandler;
        private readonly int _maxWifiConnectTimes;
        private readonly ISeleniumManipulator _seleniumManipulator;
        private readonly IWifiManager _wifiManager;
        private readonly bool _isAutoConnectWifi;
        
        
        public Worker(IConfiguration config, IWifiManager wifiManager, ISeleniumManipulator seleniumManipulator, IErrorHandler errorHandler)
        {
            _maxWifiConnectTimes = int.Parse(config[Define.MaxWifiConnectTimes]);
            _isAutoConnectWifi = bool.Parse(config[Define.IsAutoConnectWifi]);
            _wifiManager = wifiManager;
            _seleniumManipulator = seleniumManipulator;
            _errorHandler = errorHandler;
        }

        public async Task Run(string[] args)
        {
            var errorTimes = 0;
            var flag = false;

            if (!_isAutoConnectWifi)
            {
                await _seleniumManipulator.Exec();
                return;
            }
            
            while (errorTimes < _maxWifiConnectTimes)
            {
                if (await _wifiManager.ConnectToWifi())
                {
                    flag = true;
                    break;
                }
                errorTimes++;
            }
            
            if (!flag)
                _errorHandler.HandleError();
            else
                await _seleniumManipulator.Exec();

            //await _wifiManager.DisConnectWifi();
        }
    }
}