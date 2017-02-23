using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JKPeopleSearchApplication.Startup))]
namespace JKPeopleSearchApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

    }
}
