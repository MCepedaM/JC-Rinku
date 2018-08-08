using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Rinku.Presentation.Startup))]
namespace Rinku.Presentation
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
