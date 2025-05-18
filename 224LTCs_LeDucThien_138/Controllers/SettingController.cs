using _224LTCs_LeDucThien_138.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _224LTCs_LeDucThien_138.Controllers
{
    public class SettingController : Controller
    {
        private readonly ConnectionDatabase _connectionDatabase;
        private readonly TaiKhoanAdminRepos _taiKhoanAdminRepos;
        private readonly SinhVienRepos _sinhVienRepos;

        public SettingController(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
            _taiKhoanAdminRepos = new TaiKhoanAdminRepos(_connectionDatabase);
            _sinhVienRepos = new SinhVienRepos(_connectionDatabase);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminSetting()
        {
            string MaTaiKhoan = User.Identity.Name;

            var admin = _taiKhoanAdminRepos.GetAdminById(MaTaiKhoan);

            if (admin == null) {
                return RedirectToAction("Error401", "Error");
            }
            return View(admin);
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        private bool HandleAvatarUpload(
            IFormFile file,
            string folder,
            string oldFileName,
            Action<string> assignNewFileName,
            out string errorMessage)
        {
            errorMessage = null;
            if (file == null || file.Length == 0)
            {
                errorMessage = "Vui lòng chọn một ảnh hợp lệ.";
                return false;
            }

            // 1. Validate extension
            var allowed = new[] { ".jpg", ".jpeg", ".png" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowed.Contains(ext))
            {
                errorMessage = "Chỉ chấp nhận .jpg, .jpeg hoặc .png.";
                return false;
            }

            // 2. Validate size
            if (file.Length > 2 * 1024 * 1024)
            {
                errorMessage = "Ảnh phải nhỏ hơn 2MB.";
                return false;
            }

            // 3. Save new file
            var fileName = $"{Guid.NewGuid()}{ext}";
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder, fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            using (var fs = new FileStream(savePath, FileMode.Create))
                file.CopyTo(fs);

            // 4. Delete old file
            if (!string.IsNullOrEmpty(oldFileName))
            {
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder, oldFileName);
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            // 5. Gán lại tên file mới cho đối tượng
            assignNewFileName(fileName);
            return true;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult UploadAvatarTaiKhoanAdmin(IFormFile Anh)
        {
            var admin = _taiKhoanAdminRepos.GetAdminById(User.Identity.Name);
            if (admin == null) return RedirectToAction("Error401", "Error");

            if (HandleAvatarUpload(
                file: Anh,
                folder: "assets/img/user",
                oldFileName: admin.Anh,
                assignNewFileName: fn => admin.Anh = fn,
                out var error))
            {
                if (_taiKhoanAdminRepos.UpdateAvatarTaiKhoanAdmin(admin))
                    TempData["SuccessMessage"] = "Ảnh đã được tải lên thành công!";
                else
                    return RedirectToAction("Error401", "Error");
            }
            else
            {
                TempData["ErrorMessage"] = error;
            }
            return RedirectToAction("AdminSetting", "Setting");
        }

        [Authorize(Roles = "SinhVien")]
        public IActionResult SinhVienSetting()
        {
            var sv = _sinhVienRepos.GetSinhVienById(User.Identity.Name);

            if (sv == null)
            {
                return RedirectToAction("Error401", "Error");
            }
            return View(sv);
        }

        [Authorize(Roles = "SinhVien")]
        public IActionResult UpdateTaiKhoanSinhVien()
        {
            {
                var sv = _sinhVienRepos.GetSinhVienById(User.Identity.Name);

                if (sv == null)
                {
                    return RedirectToAction("Error401", "Error");
                }
                return View(sv);
            }
        }

        [Authorize(Roles = "SinhVien")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaSinhVien(SinhVien sinhVien)
        {

            bool isAdded = _sinhVienRepos.UpdateSinhVien(sinhVien);

            if (isAdded)
            {
                TempData["SuccessMessage"] = "Đã được cập nhật thành công!";
                return RedirectToAction("Index", "SinhVien");
            }
            else
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật";
            }

            return RedirectToAction("Index", "SinhVien");
        }

        [Authorize(Roles = "SinhVien")]
        [HttpPost]
        public IActionResult UploadAvatarTaiKhoanSinhVien(IFormFile Anh)
        {
            var sv = _sinhVienRepos.GetSinhVienById(User.Identity.Name);
            if (sv == null) return RedirectToAction("Error401", "Error");

            if (HandleAvatarUpload(
                file: Anh,
                folder: "assets/img/user",
                oldFileName: sv.Anh,
                assignNewFileName: fn => sv.Anh = fn,
                out var error))
            {
                if (_sinhVienRepos.UpdateAvatarSinhVien(sv))
                    TempData["SuccessMessage"] = "Ảnh đã được tải lên thành công!";
                else
                    return RedirectToAction("Error401", "Error");
            }
            else
            {
                TempData["ErrorMessage"] = error;
            }

            return RedirectToAction("SinhVienSetting", "Setting");
        }
    }
}
