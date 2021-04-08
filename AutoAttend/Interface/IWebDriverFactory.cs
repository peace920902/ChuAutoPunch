using System;
using OpenQA.Selenium;

namespace AutoAttend.Interface
{
    public interface IWebDriverFactory
    {
        Lazy<IWebDriver> Create();
    }
}