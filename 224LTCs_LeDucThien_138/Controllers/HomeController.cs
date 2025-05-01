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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemPhong(PhongHoc phong)
        {
            if (ModelState.IsValid)
            {
                if (!_phongHocRepos.IsTenPhongExists(phong.TenPhong)) {

                    bool isAdded = _phongHocRepos.AddPhong(phong);

                    if (isAdded)
                    {
                        TempData["SuccessMessage"] = "Đã được thêm thành công!";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm";
                    }
                } else
                {
                    TempData["ErrorMessage"] = "Lỗi: Tên phòng đã tồn tại";
                }
            }
            return Redirect("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaPhong(PhongHoc phong)
        {
            if (ModelState.IsValid)
            {
                if (!_phongHocRepos.IsTenPhongExists(phong.TenPhong) || _phongHocRepos.IsThisPhong(phong.MaPhong, phong.TenPhong))   
                {

                    bool isAdded = _phongHocRepos.UpdatePhong(phong);

                    if (isAdded)
                    {
                        TempData["SuccessMessage"] = "Đã được cập nhật thành công!";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Lỗi: Tên phòng đã tồn tại";
                }
            }
            return Redirect("Index");
        }


        public IActionResult XoaPhong(int maPhong)
        {
            bool isDeleted = _phongHocRepos.DeletePhong(maPhong);

            if (isDeleted)
            {
                TempData["SuccessMessage"] = "Xóa thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy hoặc có lỗi khi xóa!";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult TimKiem(string TuKhoa)
        {
            List<PhongHoc> phong;
            if (string.IsNullOrWhiteSpace(TuKhoa))
            {
                phong = _phongHocRepos.GetAllPhong();
            }
            else
            {
                phong = _phongHocRepos.TimKiemPhong(TuKhoa.Trim());
            }

            if (phong == null || !phong.Any())
            {
                TempData["Message"] = "Không tìm thấy kết quả nào.";
            }

            return View(phong);
        }
    
    }
}
