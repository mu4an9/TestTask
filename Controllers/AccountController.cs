using Url_Shortener.Models;

namespace Url_Shortener.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, false, false);

            return result switch
            {
                SignInStatus.Success => RedirectToLocal(returnUrl),
                _ => View(model).WithModelError("Invalid login attempt.")
            };
        }

        [AllowAnonymous]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            return Url.IsLocalUrl(returnUrl) ? (ActionResult)Redirect(returnUrl) : RedirectToAction("Index", "Home");
        }
    }

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public ActionResult AdminAction()
        {
            return View();
        }
    }

    [Authorize(Roles = "OrdinaryUser")]
    public class UserController : Controller
    {
        public ActionResult UserAction()
        {
            return View();
        }
    }

    [Authorize(Roles = "Admin,OrdinaryUser")]
    [ApiController]
    [Route("api")]
    public class UrlController : ControllerBase
    {
        private readonly IUrlRepository _urlRepository;

        public UrlController(IUrlRepository urlRepository)
        {
            _urlRepository = urlRepository;
        }

        [HttpGet("check-unique-url")]
        public ActionResult CheckUniqueUrl(string originalUrl)
        {
            bool isUnique = !_urlRepository.UrlExists(originalUrl);
            return Ok(new { isUnique });
        }

        [HttpPost("add-url")]
        public ActionResult AddUrl([FromBody] UrlModel model)
        {
            _urlRepository.AddUrl(model);
            return Ok();
        }

        [HttpGet("url-info/{id}")]
        public ActionResult GetUrlInfo(int id)
        {
            var urlInfo = _urlRepository.GetUrlInfo(id);

            return urlInfo != null ? Ok(urlInfo) : NotFound();
        }
    }

    public static class ControllerExtensions
    {
        public static ActionResult WithModelError(this ActionResult result, string errorMessage)
        {
            if (result is ViewResult viewResult)
            {
                viewResult.ModelState.AddModelError(string.Empty, errorMessage);
            }
            return result;
        }
    }
}
