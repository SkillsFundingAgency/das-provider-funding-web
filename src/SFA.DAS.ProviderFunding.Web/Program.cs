using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;

namespace SFA.DAS.ProviderFunding.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Info("Starting up host");
                var host = CreateWebHostBuilder(args).Build();

                host.Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Fatal(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                global::NLog.LogManager.Shutdown();
            }
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("https://localhost:5030")
                .UseNLog()
                .ConfigureLogging(b => b.ConfigureRecruitLogging());

    }
}
