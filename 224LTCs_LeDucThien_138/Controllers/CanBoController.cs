using _224LTCs_LeDucThien_138.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _224LTCs_LeDucThien_138.Controllers
{
    public class CanBoController : Controller
    {
        private readonly ConnectionDatabase _connectionDatabase;
        private readonly CanBoRepos _canBoRepos;
        private readonly KhoaRepos _khoaRepos;
        private readonly HocViRepos _hocViRepos;
        private readonly ChucVuRepos _chucVuRepos;

        public CanBoController(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
            _canBoRepos = new CanBoRepos(connectionDatabase);
            _khoaRepos = new KhoaRepos(connectionDatabase);
            _hocViRepos = new HocViRepos(connectionDatabase);
            _chucVuRepos = new ChucVuRepos(connectionDatabase); 
        }

        [HttpGet]
        public IActionResult GetALLKhoa()
        {
            var list = _khoaRepos.GetAllKhoa();
            return Json(list);
        }

        [HttpGet]
        public IActionResult GetALLChucVu()
        {
            var list = _chucVuRepos.GetAllChucVu();
            return Json(list);
        }

        [HttpGet]
        public IActionResult GetALLHocVi()
        {
            var list = _hocViRepos.GetAllHocVi();
            return Json(list);
        }

        public IActionResult Index()
        {
            var cb = _canBoRepos.GetAllCanBo();
            return View(cb);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ThemCanBo()
        { 
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemCanBo(CanBo canBo, int? maKhoa, int? maHocVi, int? maChucVu)
        {
            if (maHocVi == null || maChucVu == null)
            {
                TempData["ErrorMessage"] = "Có lỗi khi thêm chức vụ và học vị";
                return RedirectToAction("Index", "CanBo");
            }

            canBo.MaKhoa = maKhoa;
            canBo.MaChucVu = maChucVu;
            canBo.MaHocVi = maHocVi;

            var cv = _chucVuRepos.GetChucVuByID((int)maChucVu);
            if (!string.IsNullOrEmpty(cv.TenChucVu) && cv.TenChucVu.ToLower().Trim() == "giảng viên"
                || !string.IsNullOrEmpty(cv.TenChucVu) && cv.TenChucVu.ToLower().Trim() == "trưởng khoa"
                || !string.IsNullOrEmpty(cv.TenChucVu) && cv.TenChucVu.ToLower().Trim() == "phó khoa")
            {
                if (maKhoa == null)
                {
                    TempData["ErrorMessage"] = "Giảng viên bắt buộc phải chọn khoa";
                    return RedirectToAction("Index", "CanBo");
                }
            }

            bool isAdded = _canBoRepos.AddCanBo(canBo);

            if (isAdded)
            {
                TempData["SuccessMessage"] = "Đã được thêm thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm";
            }

            return RedirectToAction("Index", "CanBo");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult SuaCanBo(string maCB)
        {
            var sv = _canBoRepos.GetCanBoById(maCB);
            if (sv == null)
            {
                TempData["ErrorMessage"] = "Sinh viên không tồn tại.";
                return RedirectToAction("Index");
            }
            return View(sv);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaCanBo(CanBo canBo, int? maKhoa, int? maHocVi, int? maChucVu)
        {
            if (maHocVi == null || maChucVu == null)
            {
                TempData["ErrorMessage"] = "Có lỗi khi thay đổi chức vụ và học vị";
                return RedirectToAction("Index", "CanBo");
            }

            canBo.MaKhoa = maKhoa;
            canBo.MaChucVu = maChucVu;
            canBo.MaHocVi = maHocVi;

            var cv = _chucVuRepos.GetChucVuByID((int)maChucVu);
            if (!string.IsNullOrEmpty(cv.TenChucVu) && cv.TenChucVu.ToLower().Trim() == "giảng viên"
                || !string.IsNullOrEmpty(cv.TenChucVu) && cv.TenChucVu.ToLower().Trim() == "trưởng khoa"
                || !string.IsNullOrEmpty(cv.TenChucVu) && cv.TenChucVu.ToLower().Trim() == "phó khoa")
            {
                if (maKhoa == null)
                {
                    TempData["ErrorMessage"] = "Giảng viên bắt buộc phải chọn khoa";
                    return RedirectToAction("Index", "CanBo");
                }
            }

            bool isAdded = _canBoRepos.UpdateCanBo(canBo);

            if (isAdded)
            {
                TempData["SuccessMessage"] = "Đã được cập nhật thành công!";
                return RedirectToAction("Index", "CanBo");
            }
            else
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật";
            }

            return RedirectToAction("Index", "CanBo");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult XoaCanBo(string maCB)
        {
            bool isDeleted = _canBoRepos.DeleteCanBo(maCB);

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
        public IActionResult TimKiem(int? maKhoa = null, int? maHocVi = null, int? maChucVu = null, string? TuKhoa = null)
        {
            var cb = _canBoRepos.GetCanBoFiltered(maKhoa, maHocVi, maChucVu, TuKhoa);
            return View(cb);
        }

        [HttpGet]
        public IActionResult XemChiTietCanBo(string maCB)
        {
            var sv = _canBoRepos.GetCanBoById(maCB);
            if (sv == null)
            {
                TempData["ErrorMessage"] = "Sinh viên không tồn tại.";
                return RedirectToAction("Index");
            }
            return View(sv);
        }
    }
}
