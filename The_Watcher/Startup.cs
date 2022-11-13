using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(The_Watcher.Startup))]
namespace The_Watcher
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
