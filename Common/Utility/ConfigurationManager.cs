using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Common.Utility
{
    public static class AppSettingsConfigurationManager
    {
        public static IConfiguration AppSetting { get; }
        static AppSettingsConfigurationManager()
        {
            AppSetting = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
        }
    }
}
