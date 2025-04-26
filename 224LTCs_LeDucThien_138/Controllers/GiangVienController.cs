using Microsoft.AspNetCore.Mvc;

namespace _224LTCs_LeDucThien_138.Controllers
{
    public class GiangVienController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public GiangVienController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TimKiem()
        {
            return View();
        }
    }
}
