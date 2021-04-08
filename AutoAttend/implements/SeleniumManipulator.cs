using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoAttend.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace AutoAttend.implements
{
    public class SeleniumManipulator : ISeleniumManipulator
    {
        private const string UserName = "UserProfile:UserName";
        private const string Password = "UserProfile:Password";
        private const string AuthName = "NetworkProfile:Account";
        private const string AuthPass = "NetworkProfile:Password";
        private readonly string _chuUrl;
        private readonly IConfiguration _configuration;
        private readonly IWebDriver _driver;
        private readonly IErrorHandler _errorHandler;
        private readonly Lazy<IWebDriver> _wifiDriver;
        private readonly ILogger<SeleniumManipulator> _logger;

        public SeleniumManipulator(IConfiguration configuration, IWebDriverFactory webDriverFactory, IWebDriver driver, IErrorHandler errorHandler, ILogger<SeleniumManipulator> logger)
        {
            _configuration = configuration;
            if(bool.Parse(_configuration[Define.IsAutoConnectWifi])) _wifiDriver = webDriverFactory.Create();
            _driver = driver;
            _errorHandler = errorHandler;
            _logger = logger;
            _chuUrl = _configuration["ChuUrl"];
        }

        public async Task Exec()
        {
            if (NavToInternet())
                AutoLogin();

            if (bool.Parse(_configuration[Define.Auto]))
                _driver.Close();
        }

        public void LoginWifi()
        {
            _logger.Log(LogLevel.Information, "start wifi login");
            _wifiDriver.Value.Navigate().GoToUrl(_configuration[Define.WifiUrl]);
            _wifiDriver.Value.FindElement(By.Id("auth_user")).SendKeys(_configuration[AuthName]);
            _wifiDriver.Value.FindElement(By.Name("auth_pass")).SendKeys(_configuration[AuthPass]);
            ClickButton(_wifiDriver.Value, By.Name("accept"), "wifi log in");
            _logger.Log(LogLevel.Information, "stop wifi login");
            _wifiDriver.Value.Close();
        }

        private bool NavToInternet()
        {
            if (NavToChu()) return true;
            Thread.Sleep(3000);
            return false;
        }

        private bool NavToChu()
        {
            _driver.Navigate().GoToUrl(_chuUrl);
            var findElements = _driver.FindElements(By.XPath(@"/html/body/div[3]/div[1]/div/div[2]/div/div/h1/span"));
            return _driver.Url == _chuUrl &&
                   findElements != null &&
                   findElements.Any() &&
                   !string.IsNullOrEmpty(findElements.First().Text);
        }

        private void AutoLogin()
        {
            _logger.Log(LogLevel.Information, "Start attend");
            _driver.FindElement(By.Id("txtAccount")).SendKeys(_configuration[UserName]);
            _driver.FindElement(By.Id("txtPwd")).SendKeys(_configuration[Password]);

            ClickButton(_driver, By.XPath(@"/html/body/div[3]/div[1]/div/div[2]/div/div/div/form/fieldset/input[1]"), "submit");
            Thread.Sleep(1000);
            ClickButton(_driver, By.XPath(@"/html/body/div[2]/div[2]/div/div/section[1]/div/aside/section/div/div/div[1]/div[2]/div/div/div[1]/div/ul/li/div/div[1]/a"), "course");
            Thread.Sleep(1000);
            ClickButton(_driver, By.XPath("/html/body/div[1]/div[2]/div/div/section[1]/div/div[2]/ul/li/div[3]/ul/li[2]/div/div/div[2]/div/a"), "attend");
            if (!bool.Parse(_configuration[Define.Auto])||!bool.Parse(_configuration[Define.IsAutoConnectWifi]))
            {
                Thread.Sleep(int.Parse(_configuration[Define.ManualTimeOut]));
                return;
            }
            
            ClickButton(_driver, By.XPath(@"/html/body/div[1]/div[2]/div/div/section/div[1]/div[1]/a"), "punchCard");
            if (bool.Parse(_configuration[Define.WindowMode]))
                Thread.Sleep(int.Parse(_configuration[Define.WindowModeCheckedTime]));

            _logger.Log(LogLevel.Information, "stop attend");
        }

        private void ClickButton(IWebDriver webDriver, By by, string buttonName)
        {
            while (CheckIfNetworkChanged(webDriver))
            {
                Thread.Sleep(3000);
                webDriver.Navigate().Refresh();
            }

            var button = webDriver.FindElements(by);

            while (button == null || !button.Any())
            {
                _errorHandler.LogErrorAndDelay($"{buttonName} button not found");
                if (_errorHandler.CheckError()) _errorHandler.HandleError();
                button = webDriver.FindElements(by);
            }

            button.First().Click();
        }

        private bool CheckIfNetworkChanged(IWebDriver webDriver)
        {
            var errorCodes = webDriver.FindElements(By.ClassName("error-code"));
            return errorCodes == null || errorCodes.Any();
        }
    }
}