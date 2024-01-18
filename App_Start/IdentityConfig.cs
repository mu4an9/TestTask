using Microsoft.AspNetCore.Identity;
using Owin;

namespace Url_Shortener.App_Start
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            // Конфигурация контекста и манеджеров
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // ... Другие настройки ...

            // Настройка аутентификации с использованием куки
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Позволяет приложению проверить, должен ли пользователь быть переадресован на страницу входа
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });

            CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            // Создание ролей
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("OrdinaryUser"))
            {
                var role = new IdentityRole();
                role.Name = "OrdinaryUser";
                roleManager.Create(role);
            }
        }
    }

}
