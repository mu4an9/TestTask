using Microsoft.AspNet.Identity;
using Owin;
using Url_Shortener.Data;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;

namespace Url_Shortener
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }
    }
    class Program
    {
        static void Main()
        {
            using (var context = new AppDbContext())
            {
                // Используйте контекст для взаимодействия с базой данных
                var users = context.Users.ToList();
            }

            // Остальной код программы
        }
    }

}
