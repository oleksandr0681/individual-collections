using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IndividualCollectionsWeb.Startup))]
namespace IndividualCollectionsWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
