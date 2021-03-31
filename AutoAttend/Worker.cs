﻿using System.Threading.Tasks;
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

        public Worker(IConfiguration config, IWifiManager wifiManager, ISeleniumManipulator seleniumManipulator, IErrorHandler errorHandler)
        {
            _maxWifiConnectTimes = int.Parse(config[Define.MaxWifiConnectTimes]);
            _wifiManager = wifiManager;
            _seleniumManipulator = seleniumManipulator;
            _errorHandler = errorHandler;
        }

        public async Task Run(string[] args)
        {
            var errorTimes = 0;
            var flag = false;
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
        }
    }
}