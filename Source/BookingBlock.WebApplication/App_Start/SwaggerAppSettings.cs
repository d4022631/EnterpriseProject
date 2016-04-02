namespace BookingBlock.WebApplication
{
    public static class SwaggerAppSettings
    {
        public static string XmlCommentsPath
        {
            get
            {
                return AppSettings.GetValueOrDefault("Swagger.XmlCommentsPath", @"BookingBlock.WebApplication.XML");
            }
        }

    }
}