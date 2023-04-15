using DatabaseConfigurations.Models;
using DatabaseConfigurations.Services;
using Microsoft.Extensions.Logging;
using System.Web.Mvc;

namespace DatabaseConfigurations.Controllers
{
    public class FooController : Controller
    {
        private readonly ILogger<FooController> _logger;
        private readonly ICacheHandler _cacheHandler;

        public FooController(ILogger<FooController> logger, ICacheHandler cacheHandler)
        {
            _logger = logger;
            _cacheHandler = cacheHandler;
        }

        [HttpGet]
        [Route("foo/configs", Name = "foo-configs")]
        public ActionResult RenderFooConfigurations()
        {
            _logger.LogInformation("FooController()");
            FooConfiguration fc = new FooConfiguration();
            fc = _cacheHandler.GetConfiguration<FooConfiguration>(fc.Name);
            return View("Foo", fc);
        }
    }
}