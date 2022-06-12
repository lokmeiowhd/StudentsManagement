using System;
using System.Collections.Generic;
using System.Linq;
using NLog.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MockSchoolManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging((hostingContext, logging) =>
            {
                //logging.AddConsole();
                //logging.AddDebug();
                //logging.AddEventSourceLogger();

                //ɾ������Ĭ����־��¼�ṩ����
                logging.ClearProviders();
                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                //���NLog��Ϊ��־�ṩ����
                logging.AddNLog();
            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
