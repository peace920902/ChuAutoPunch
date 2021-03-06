using System;
using System.Threading.Tasks;
using AutoAttend.implements;
using AutoAttend.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Serilog;

namespace AutoAttend
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .WriteTo.File(@"C:\log.txt")
                    .MinimumLevel.Information()
                    .CreateLogger();
                Log.Logger.Information("Auto attend start");

                var services = new ServiceCollection();
                ConfigureService(services);
                
                await services.BuildServiceProvider().GetService<Worker>().Run(args);
            }
            catch (Exception e)
            {
                Log.Logger.Fatal(e.ToString());
            }
            finally
            {
                Log.Logger.Information("Auto attend down");
            }
        }

        private static void ConfigureService(IServiceCollection services)
        {
            services.AddLogging(x =>
            {
                x.AddSerilog();
            });
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsetting.json", false, true)
                .Build();
            services.AddSingleton<IConfiguration>(config);
            //services.Configure<UserProfile>(config.GetSection(Define.UserProfile));
            var chromeOptions = new ChromeOptions();
            if (!bool.Parse(config[Define.WindowMode]))
                chromeOptions.AddArguments("--headless");
            services.AddSingleton<IWebDriverFactory, WebDriverFactory>();
            services.AddSingleton<IWebDriver>(new ChromeDriver(chromeOptions));
            services.AddSingleton<IErrorHandler, ErrorHandler>();
            services.AddSingleton<ISeleniumManipulator, SeleniumManipulator>();
            services.AddSingleton<IWifiManager, WifiManager>();
            services.AddSingleton<Worker>();
        }
    }
}