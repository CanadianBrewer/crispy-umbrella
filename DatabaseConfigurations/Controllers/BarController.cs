using DatabaseConfigurations.Models;
using DatabaseConfigurations.Services;
using Microsoft.Extensions.Logging;
using System.Web.Mvc;

namespace DatabaseConfigurations.Controllers
{
    public class BarController : Controller
    {
        private readonly ILogger<BarController> _logger;
        private readonly ICacheHandler _cacheHandler;

        public BarController(ILogger<BarController> logger, ICacheHandler cacheHandler)
        {
            _logger = logger;
            _cacheHandler = cacheHandler;
        }

        [HttpGet]
        [Route("bar/configs", Name = "bar-configs")]
        public ActionResult RenderBarConfigurations()
        {
            _logger.LogInformation("BarController()");
            BarConfiguration bc = new BarConfiguration();
            bc = _cacheHandler.GetConfiguration<BarConfiguration>(bc.Name);
            return View("Bar", bc);

        }
    }
}