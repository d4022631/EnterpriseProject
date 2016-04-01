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
}
