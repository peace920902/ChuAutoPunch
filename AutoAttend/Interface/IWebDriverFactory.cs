using OpenQA.Selenium;

namespace AutoAttend.Interface
{
    public interface IWebDriverFactory
    {
        IWebDriver Create();
    }
}