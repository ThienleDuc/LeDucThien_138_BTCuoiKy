using _224LTCs_LeDucThien_138.Models;
using Microsoft.AspNetCore.Mvc;

namespace _224LTCs_LeDucThien_138.Controllers
{
    public class HomeController : Controller
    {
        private readonly ConnectionDatabase _connectionDatabase;
        private readonly PhongHocRepos _phongHocRepos;

        public HomeController(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
            _phongHocRepos = new PhongHocRepos(_connectionDatabase);
        }

        public IActionResult Index()
        {
            var phong = _phongHocRepos.GetAllPhong();
            if (phong == null) {
                return RedirectToAction("Error401", "Error");
            }
            return View(phong);
        }

        public IActionResult TimKiem()
        {
            return View();
        }
    
    }
}
