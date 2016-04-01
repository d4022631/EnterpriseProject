namespace BookingBlock.WebApplication
{
    /// <summary>
    /// This class accesses site specific settings (such as the site Url) from the app settings stored in the web.config.
    /// </summary>
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
}