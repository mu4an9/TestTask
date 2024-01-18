using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Identity;
using Microsoft.Owin.Security;
using System.Web.Mvc;
using Url_Shortener.Models;

namespace Url_Shortener.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<ApplicationUser, string> _signInManager;

        public AccountController()
        {
        }

        [Authorize(Roles = "Admin")]
        public class AdminController : Controller
        {
            public ActionResult AdminAction()
            {
                // Код доступный только для пользователей с ролью "Admin"
                return View();
            }
        }
        public class UserController : Controller
        {
            [Authorize(Roles = "OrdinaryUser")]
            public ActionResult UserAction()
            {
                // Код доступный только для пользователей с ролью "OrdinaryUser"
                return View();
            }
        }


        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, "OrdinaryUser");

                    // Присвоение роли Admin, если условие выполняется (например, по email, логину и т. д.)
                    if (model.Email == "admin@example.com")
                    {
                        await UserManager.AddToRoleAsync(user.Id, "Admin");
                    }

                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    return RedirectToAction("Index", "Home");
                }

                AddErrors(result);
            }

            // Если что-то пошло не так, вернуть представление с ошибками
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public AccountController(SignInManager<ApplicationUser, string> signInManager)
        {
            _signInManager = signInManager;
        }

        public SignInManager<ApplicationUser, string> SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<SignInManager<ApplicationUser, string>>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.Login, model.Password, isPersistent: false, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        // GET: /Account/Logout
        [AllowAnonymous]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
