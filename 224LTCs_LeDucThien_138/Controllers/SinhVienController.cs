using _224LTCs_LeDucThien_138.Models;
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
        public IActionResult GetChuyenNganhByKhoa(int maKhoa)
        {
            var list = _chuyenNganhRepos.GetChuyenNganhByKhoa(maKhoa);
            return Json(list);
        }

        [HttpGet]
        public IActionResult GetLopSinhHoat(int maNganh, string maNK)
        {
            var list = _lopSinhHoatRepos.GetLopSinhHoatById(maNganh, maNK);
            return Json(list);
        }

        private List<SinhVien> LayDanhSachSinhVien(int makhoa, int maNganh, string maLSH, string maNienKhoa)
        {
            if (CoDayDuThongTinLSH(maNienKhoa, makhoa, maNganh, maLSH))
                return LayTheoLop(maNienKhoa, makhoa, maNganh, maLSH);

            if (CoThongTinChuyenNganh(maNienKhoa, makhoa, maNganh))
                return LayTheoChuyenNganh(maNienKhoa, makhoa, maNganh);

            if (CoThongTinKhoa(maNienKhoa, makhoa))
                return LayTheoKhoa(maNienKhoa, makhoa);

            if (!string.IsNullOrEmpty(maNienKhoa))
                return LayTheoNienKhoa(maNienKhoa);

            return LayTatCaSinhVien();
        }

        private bool CoDayDuThongTinLSH(string maNienKhoa, int makhoa, int maNganh, string maLSH)
            => !string.IsNullOrEmpty(maNienKhoa) && makhoa > 0 && maNganh > 0 && !string.IsNullOrEmpty(maLSH);

        private bool CoThongTinChuyenNganh(string maNienKhoa, int makhoa, int maNganh)
            => !string.IsNullOrEmpty(maNienKhoa) && makhoa > 0 && maNganh > 0;

        private bool CoThongTinKhoa(string maNienKhoa, int makhoa)
            => !string.IsNullOrEmpty(maNienKhoa) && makhoa > 0;

        private List<SinhVien> LayTheoNienKhoa(string maNienKhoa)
            => _sinhVienRepos.GetSinhVienByNienKhoa(maNienKhoa);

        private List<SinhVien> LayTheoKhoa(string maNienKhoa, int makhoa)
            => _sinhVienRepos.GetSinhVienByKhoa(maNienKhoa, makhoa);

        private List<SinhVien> LayTheoChuyenNganh(string maNienKhoa, int makhoa, int maNganh)
            => _sinhVienRepos.GetSinhVienByChuyenNganh(maNienKhoa, makhoa, maNganh);

        private List<SinhVien> LayTheoLop(string maNienKhoa, int makhoa, int maNganh, string maLSH)
            => _sinhVienRepos.GetSinhVienByLSH(maNienKhoa, makhoa, maNganh, maLSH);

        private List<SinhVien> LayTatCaSinhVien()
            => _sinhVienRepos.GetAllSinhVien();

        [HttpGet]
        public IActionResult Index(int makhoa, int maNganh, string maLSH, string maNienKhoa)
        {
            var sv = LayDanhSachSinhVien(makhoa, maNganh, maLSH, maNienKhoa);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("TableSinhVien", sv);
            }

            return View(sv);
        }

        public IActionResult ThemSinhVien()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemSinhVien(SinhVien sinhVien, string maNK, int maKhoa, int maNganh, string maLSH)
        {
            if (string.IsNullOrEmpty(maNK) || maKhoa <= 0 || maNganh <= 0 || string.IsNullOrEmpty(maLSH)) 
            {
                TempData["ErrorMessage"] = "Có lỗi khi thêm niên khóa, Khoa, ngành và lớp sinh hoạt";
            } else
            {
                sinhVien.MaLSH = maLSH;
                bool isAdded = _sinhVienRepos.AddSinhVien(sinhVien, maNK, maKhoa, maNganh, maLSH);

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

        public IActionResult TimKiem()
        {
            return View();
        }
    }
}
