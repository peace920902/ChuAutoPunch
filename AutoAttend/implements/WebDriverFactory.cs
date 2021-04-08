using System;
using AutoAttend.Interface;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutoAttend.implements
{
    public class WebDriverFactory : IWebDriverFactory
    {
        private readonly IConfiguration _configuration;

        public WebDriverFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Lazy<IWebDriver> Create()
        {
            var chromeOptions = new ChromeOptions();
            if (!bool.Parse(_configuration[Define.WindowMode]))
                chromeOptions.AddArguments("--headless");
            return new Lazy<IWebDriver>(new ChromeDriver(chromeOptions));
        }
    }
}