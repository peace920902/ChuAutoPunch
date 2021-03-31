using System;
using System.Threading;
using AutoAttend.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AutoAttend.implements
{
    public class ErrorHandler : IErrorHandler
    {
        private static int _errorTimes;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ErrorHandler> _logger;
        private readonly int _maxErrorTimes;


        public ErrorHandler(IConfiguration configuration, ILogger<ErrorHandler> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _maxErrorTimes = int.Parse(configuration[Define.MaxErrorTimes]);
        }

        public void LogErrorAndDelay(string error, int delaySecond = 3000)
        {
            _logger.Log(LogLevel.Error, error);
            _errorTimes++;
            Thread.Sleep(delaySecond);
        }

        public bool CheckError() => _errorTimes >= _maxErrorTimes;

        public void HandleError()
        {
            //log
            _logger.Log(LogLevel.Critical, "Too many error");
            Environment.Exit(-1);
        }
    }
}