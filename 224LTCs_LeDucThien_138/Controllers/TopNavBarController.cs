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
                return RedirectToAction("Error401", "Error");
            }
            return View(admin);
        }

        public IActionResult UpdateTaiKhoanAdmin()
        {
            string MaTaiKhoan = _cookieHelper.GetCookie("MaTaiKhoan");

            var admin = _taiKhoanAdminRepos.GetAdminById(MaTaiKhoan);

            if (admin == null)
            {
                return RedirectToAction("Error401", "Error");
            }
            return View(admin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateTaiKhoanAdmin(TaiKhoanAdmin taiKhoanAdmin)
        {
            string MaTaiKhoan = _cookieHelper.GetCookie("MaTaiKhoan");
            taiKhoanAdmin.MaTaiKhoan = MaTaiKhoan;

            if (!string.IsNullOrEmpty(MaTaiKhoan)) {

                bool isUpdated = _taiKhoanAdminRepos.UpdateTaiKhoanAdmin(taiKhoanAdmin);

                if (isUpdated)
                {
                    TempData["SuccessMessage"] = "Cập nhật thành công!";
                    return RedirectToAction("AdminSetting", "TopNavBar");
                }

            }

            return RedirectToAction("Error401", "Error");
        }


        [HttpPost]
        public IActionResult UploadAvatar(IFormFile Anh)
        {
            string MaTaiKhoan = _cookieHelper.GetCookie("MaTaiKhoan");

            var admin = _taiKhoanAdminRepos.GetAdminById(MaTaiKhoan);

            if (admin == null)
            {
                return RedirectToAction("Error401", "Error");
            }

            if (Anh != null && Anh.Length > 0)
            {
                // Giới hạn định dạng
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(Anh.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    TempData["ErrorMessage"] = "Chỉ chấp nhận định dạng ảnh .jpg, .jpeg hoặc .png.";
                    return RedirectToAction("AdminSetting","TopNavBar");
                }

                // Giới hạn kích thước 2MB
                if (Anh.Length > 2 * 1024 * 1024)
                {
                    TempData["ErrorMessage"] = "Ảnh phải có kích thước nhỏ hơn 2MB.";
                    return RedirectToAction("AdminSetting", "TopNavBar");
                }

                var fileName = Path.GetFileName(Anh.FileName); 
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/img", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    Anh.CopyTo(stream);
                }

                // Cập nhật tên file ảnh vào đối tượng admin
                admin.Anh = fileName;


                // Gọi hàm cập nhật vào database
                bool isUpdated = _taiKhoanAdminRepos.UpdateAvatarTaiKhoanAdmin(admin);

                if (isUpdated)
                {
                    TempData["SuccessMessage"] = "Ảnh đã được tải lên thành công!";
                }
                else
                {
                    return RedirectToAction("Error401", "Error");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Vui lòng chọn một ảnh hợp lệ.";
            }

            return RedirectToAction("AdminSetting", "TopNavBar");
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
