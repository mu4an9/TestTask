using Url_Shortener.Models;

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
    public IActionResult CheckUniqueUrl(string originalUrl)
    {
        bool isUnique = !_urlRepository.UrlExists(originalUrl);
        return Ok(new { isUnique });
    }

    [HttpPost("add-url")]
    public IActionResult AddUrl([FromBody] UrlModel model)
    {
        _urlRepository.AddUrl(model);
        return Ok();
    }
}

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("privacy")]
    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet("error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
