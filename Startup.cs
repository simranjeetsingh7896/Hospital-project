using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(team2Geraldton.Startup))]
namespace team2Geraldton
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
