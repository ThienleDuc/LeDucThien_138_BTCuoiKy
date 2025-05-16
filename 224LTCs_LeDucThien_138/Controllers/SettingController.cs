using _224LTCs_LeDucThien_138.Models;
using Microsoft.AspNetCore.Mvc;

namespace _224LTCs_LeDucThien_138.Controllers
{
    public class SettingController : Controller
    {
        private readonly ConnectionDatabase _connectionDatabase;
        private readonly TaiKhoanAdminRepos _taiKhoanAdminRepos;

        public SettingController(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
            _taiKhoanAdminRepos = new TaiKhoanAdminRepos(_connectionDatabase);
        }

        public IActionResult AdminSetting()
        {
            string MaTaiKhoan = User.Identity.Name;

            var admin = _taiKhoanAdminRepos.GetAdminById(MaTaiKhoan);

            if (admin == null) {
                return RedirectToAction("Error401", "Error");
            }
            return View(admin);
        }

        public IActionResult UpdateTaiKhoanAdmin()
        {
            string MaTaiKhoan = User.Identity.Name;

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
            string MaTaiKhoan = User.Identity.Name; ;
            taiKhoanAdmin.MaTaiKhoan = MaTaiKhoan;

            if (!string.IsNullOrEmpty(MaTaiKhoan)) {

                bool isUpdated = _taiKhoanAdminRepos.UpdateTaiKhoanAdmin(taiKhoanAdmin);

                if (isUpdated)
                {
                    TempData["SuccessMessage"] = "Cập nhật thành công!";
                    return RedirectToAction("AdminSetting", "Setting");
                }

            }

            return RedirectToAction("Error401", "Error");
        }


        [HttpPost]
        public IActionResult UploadAvatar(IFormFile Anh)
        {
            string MaTaiKhoan = User.Identity.Name;

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
                    return RedirectToAction("AdminSetting","Setting");
                }

                // Giới hạn kích thước 2MB
                if (Anh.Length > 2 * 1024 * 1024)
                {
                    TempData["ErrorMessage"] = "Ảnh phải có kích thước nhỏ hơn 2MB.";
                    return RedirectToAction("AdminSetting", "Setting");
                }

                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(Anh.FileName)}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/img/user", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    Anh.CopyTo(stream);
                }

                // Xóa ảnh cũ nếu tồn tại
                if (!string.IsNullOrEmpty(admin.Anh))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/img/user", admin.Anh);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
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

            return RedirectToAction("AdminSetting", "Setting");
        }
    }
}
