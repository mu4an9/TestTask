using Url_Shortener.Models;

namespace Url_Shortener.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            var model = new UrlShortenerInfoModel
            {
                AlgorithmDescription = "Опис алгоритму URL Shortener."
            };

            return View(model);
        }
    }
}
