using _224LTCs_LeDucThien_138.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _224LTCs_LeDucThien_138.Controllers
{
    public class SinhVienController : Controller
    {
        private readonly ConnectionDatabase _connectionDatabase;
        private readonly SinhVienRepos _sinhVienRepos;
        private readonly KhoaRepos _khoaRepos;
        private readonly ChuyenNganhRepos _chuyenNganhRepos;
        private readonly NienKhoaRepos _nienKhoaRepos;
        private readonly LopSinhHoatRepos _lopSinhHoatRepos;


        public SinhVienController(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
            _sinhVienRepos = new SinhVienRepos(_connectionDatabase);
            _khoaRepos = new KhoaRepos(_connectionDatabase);
            _chuyenNganhRepos = new ChuyenNganhRepos(_connectionDatabase);
            _nienKhoaRepos = new NienKhoaRepos(_connectionDatabase);
            _lopSinhHoatRepos = new LopSinhHoatRepos(_connectionDatabase);
        }

        [HttpGet]
        public IActionResult GetALLKhoa()
        {
            var list = _khoaRepos.GetAllKhoa();
            return Json(list);
        }

        [HttpGet]
        public IActionResult GetALLNienKhoa()
        {
            var list= _nienKhoaRepos.GetAllNienKhoa();
            return Json(list);
        }

        [HttpGet]
        public IActionResult GetChuyenNganhByKhoa(int? maKhoa)
        {
            var list = _chuyenNganhRepos.GetChuyenNganhByKhoa(maKhoa);
            return Json(list);
        }

        [HttpGet]
        public IActionResult GetLopSinhHoat(int? maNganh, string? maNK)
        {
            var list = _lopSinhHoatRepos.GetLopSinhHoatById(maNganh, maNK);
            return Json(list);
        }

        public IActionResult Index()
        {
            var sv = _sinhVienRepos.GetAllSinhVien();

            return View(sv);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ThemSinhVien()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemSinhVien(SinhVien sinhVien, string? maNK, int? maKhoa, int? maNganh, string? maLSH)
        {
            if (string.IsNullOrEmpty(maNK) || maKhoa == null || maNganh == null || string.IsNullOrEmpty(maLSH)) 
            {
                TempData["ErrorMessage"] = "Có lỗi khi thêm niên khóa, Khoa, ngành và lớp sinh hoạt";
            } else
            {
                sinhVien.MaLSH = maLSH;
                bool isAdded = _sinhVienRepos.AddSinhVien(sinhVien, maNK, maKhoa, maNganh,  maLSH);

                if (isAdded)
                {
                    TempData["SuccessMessage"] = "Đã được thêm thành công!";
                    return RedirectToAction("Index", "SinhVien");
                }
                else
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm";
                }
            }

            return RedirectToAction("Index", "SinhVien");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult SuaSinhVien(string maSV)
        {
            var sv = _sinhVienRepos.GetSinhVienById(maSV);
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

        [Authorize(Roles = "Admin")]
        public IActionResult XoaSinhVien(string maSV)
        {
            bool isDeleted = _sinhVienRepos.DeleteSinhVien(maSV);

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
        public IActionResult TimKiem(string? maNK,int? maKhoa, int? maNganh, string? maLSH, string? TuKhoa)
        {
            var sv = _sinhVienRepos.GetSinhVienFiltered(maNK, maKhoa, maNganh, maLSH, TuKhoa);

            return View(sv);
        }

        [HttpGet]
        public IActionResult XemChiTietSinhVien(string maSV)
        {
            var sv = _sinhVienRepos.GetSinhVienById(maSV);
            if (sv == null)
            {
                TempData["ErrorMessage"] = "Sinh viên không tồn tại.";
                return RedirectToAction("Index");
            }
            return View(sv);
        }
    }
}
