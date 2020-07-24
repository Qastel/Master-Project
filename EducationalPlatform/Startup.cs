using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EducationalPlatform.Startup))]
namespace EducationalPlatform
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
