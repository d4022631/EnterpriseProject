namespace BookingBlock.WebApplication
{
    /// <summary>
    /// A class for working with the app settings stored in the web.config.
    /// </summary>
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