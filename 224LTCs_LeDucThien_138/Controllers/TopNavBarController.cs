using Microsoft.AspNetCore.Mvc;

namespace _224LTCs_LeDucThien_138.Controllers
{
    public class TopNavBarController : Controller
    {
        public IActionResult AdminSetting()
        {
            return View();
        }

        public IActionResult GiangVienSetting()
        {
            return View();
        }

        public IActionResult SinhVienSetting()
        {
            return View();
        }
    }
}
