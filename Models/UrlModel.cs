using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Url_Shortener.Models
{
    public class UrlModel
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; }
    }

    public class UrlRepository
    {
        private readonly List<UrlModel> _urlData = new List<UrlModel>();

        public bool UrlExists(string originalUrl) => _urlData.Any(u => u.OriginalUrl == originalUrl);

        public void AddUrl(UrlModel model)
        {
            model.Id = _urlData.Count + 1;
            _urlData.Add(model);
        }
    }

    [ApiController]
    [Route("api")]
    public class UrlController : ControllerBase
    {
        private readonly UrlRepository _urlRepository = new UrlRepository();

        [HttpGet("check-unique-url")]
        public ActionResult CheckUniqueUrl(string originalUrl) => Ok(new { isUnique = !_urlRepository.UrlExists(originalUrl) });

        [HttpPost("add-url")]
        public ActionResult AddUrl([FromBody] UrlModel model)
        {
            _urlRepository.AddUrl(model);
            return Ok();
        }
    }
}
