using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoAttend.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace AutoAttend.implements
{
    public class SeleniumManipulator : ISeleniumManipulator
    {
        private readonly IConfiguration _configuration;
        private readonly IWebDriver _driver;
        private readonly IErrorHandler _errorHandler;
        private const string UserName = "UserProfile:UserName";
        private const string Password = "UserProfile:Password";
        private readonly string _chuUrl;

        public SeleniumManipulator(IConfiguration configuration, IWebDriver driver, IErrorHandler errorHandler)
        {
            _configuration = configuration;
            _driver = driver;
            _errorHandler = errorHandler;
            _chuUrl = _configuration["ChuUrl"];
        }

        public async Task Exec()
        {
            if (NavToInternet())
                AutoLogin();

            if (bool.Parse(_configuration[Define.Auto]))
                _driver.Close();
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
            return _driver.Url == _chuUrl && !string.IsNullOrEmpty(_driver.FindElement(By.XPath(@"/html/body/div[3]/div[1]/div/div[2]/div/div/h1/span")).Text);
        }

        private void AutoLogin()
        {
            _driver.FindElement(By.Id("txtAccount")).SendKeys(_configuration[UserName]);
            _driver.FindElement(By.Id("txtPwd")).SendKeys(_configuration[Password]);

            var submitBy = By.XPath(@"/html/body/div[3]/div[1]/div/div[2]/div/div/div/form/fieldset/input[1]");
            ClickButton(submitBy, "submit");
            Thread.Sleep(3000);
            var courseBy = By.XPath(@"/html/body/div[2]/div[2]/div/div/section[1]/div/aside/section/div/div/div[1]/div[2]/div/div/div[1]/div/ul/li/div/div[1]/a");
            ClickButton(courseBy, "course");
            Thread.Sleep(3000);
            var attendBy = By.XPath("/html/body/div[1]/div[2]/div/div/section[1]/div/div[2]/ul/li/div[3]/ul/li[2]/div/div/div[2]/div/a");
            ClickButton(attendBy, "attend");
            if (!bool.Parse(_configuration[Define.Auto]))
            {
                Thread.Sleep(int.Parse(_configuration[Define.ManualTimeOut]));
                return;
            }
            // var punchCardBy = By.XPath(@"/html/body/div[1]/div[2]/div/div/section/div[1]/div[1]/a");
            // ClickButton(punchCardBy, "punchCard");
            
            if(bool.Parse(_configuration[Define.WindowMode]))
                Thread.Sleep(int.Parse(_configuration[Define.WindowModeCheckedTime]));
        }

        private void ClickButton(By by, string buttonName)
        {
            var button = _driver.FindElements(by);
            while (button == null || !button.Any())
            {
                _errorHandler.LogErrorAndDelay($"{buttonName} button not found");
                if (_errorHandler.CheckError()) _errorHandler.HandleError();
                button = _driver.FindElements(by);
            }

            button.First().Click();
        }
    }
}