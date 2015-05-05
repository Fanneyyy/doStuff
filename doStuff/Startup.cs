using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(doStuff.Startup))]
namespace doStuff
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
