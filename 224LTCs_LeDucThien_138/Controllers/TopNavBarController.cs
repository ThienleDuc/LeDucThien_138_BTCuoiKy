using _224LTCs_LeDucThien_138.Models;
using Microsoft.AspNetCore.Mvc;

namespace _224LTCs_LeDucThien_138.Controllers
{
    public class TopNavBarController : Controller
    {
        private readonly ConnectionDatabase _connectionDatabase;
        private readonly TaiKhoanAdminRepos _taiKhoanAdminRepos;
        private readonly CookieHelper _cookieHelper;

        public TopNavBarController(ConnectionDatabase connectionDatabase, CookieHelper cookieHelper)
        {
            _connectionDatabase = connectionDatabase;
            _taiKhoanAdminRepos = new TaiKhoanAdminRepos(_connectionDatabase);
            _cookieHelper = cookieHelper;
        }

        public IActionResult AdminSetting()
        {
            string MaTaiKhoan = _cookieHelper.GetCookie("MaTaiKhoan");
            var admin = _taiKhoanAdminRepos.GetAdminById(MaTaiKhoan);

            if (admin == null) {
                return RedirectToAction("Error404", "Error");
            }
            return View(admin);
        }

        public IActionResult UpdateTaiKhoanAdmin(string MaTaiKhoan)
        {
            if (string.IsNullOrEmpty(MaTaiKhoan)) {
                return RedirectToAction("Error404", "Error");
            }

            var admin = _taiKhoanAdminRepos.GetAdminById(MaTaiKhoan);

            if (admin == null)
            {
                return RedirectToAction("Error404", "Error");
            }
            return View(admin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateTaiKhoanAdmin(TaiKhoanAdmin taiKhoanAdmin)
        {
            if (ModelState.IsValid)
            {
                bool isUpdated = _taiKhoanAdminRepos.UpdateTaiKhoanAdmin(taiKhoanAdmin);

                if (isUpdated)
                {
                    TempData["SuccessMessage"] = "Cập nhật thành công!";
                    return RedirectToAction("AdminSetting", "TopNavBar");
                }
                else
                {
                    TempData["ErrorMessage"] = "Có lỗi khi cập nhật!";
                }
            } else TempData["ErrorMessage"] = "Không hợp lệ!";

            return View(taiKhoanAdmin);
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
