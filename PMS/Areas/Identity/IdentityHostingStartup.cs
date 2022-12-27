using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(PMS.Areas.Identity.IdentityHostingStartup))]

namespace PMS.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}