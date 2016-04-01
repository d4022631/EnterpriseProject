using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BookingBlock.WebApplication.Startup))]
namespace BookingBlock.WebApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }

    public static class SiteAppSettings
    {
        public static string Url
        {
            get { return AppSettings.GetValueOrDefault("Site.Url", "http://localhost:37026/"); }
        }

        public static string SslUrl
        {
            get { return AppSettings.GetValueOrDefault("Site.SslUrl", "https://localhost:44383/"); }
        }
    }

    public static class AppSettings
    {
        public static string GetValue(string name)
        {
            return System.Configuration.ConfigurationManager.AppSettings[name];
        }

        public static string GetValueOrDefault(string name, string defautValue = default(string))
        {
            return GetValue(name) ?? defautValue;
        }
    }
}
