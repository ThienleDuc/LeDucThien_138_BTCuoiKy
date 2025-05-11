using _224LTCs_LeDucThien_138.Models;
using Microsoft.AspNetCore.Mvc;

namespace _224LTCs_LeDucThien_138.Controllers
{
    public class LopHocPhanController : Controller
    {
        private readonly ConnectionDatabase _connectionDatabase;
        private readonly LopHocPhanRepos _lopHocPhanRepos;

        public LopHocPhanController(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
            _lopHocPhanRepos = new LopHocPhanRepos(_connectionDatabase);
        }

        public IActionResult Index()
        {
            var lhp = _lopHocPhanRepos.GetAllLopHocPhan();
            return View(lhp);
        }

        public IActionResult TimKiem()
        {
            return View();
        }
    }
}
