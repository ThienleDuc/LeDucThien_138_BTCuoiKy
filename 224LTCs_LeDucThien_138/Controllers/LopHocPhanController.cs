using _224LTCs_LeDucThien_138.Models;
using Microsoft.AspNetCore.Mvc;

namespace _224LTCs_LeDucThien_138.Controllers
{
    public class LopHocPhanController : Controller
    {
        private readonly ConnectionDatabase _connectionDatabase;
        private readonly LopHocPhanRepos _lopHocPhanRepos;
        private readonly KhoaRepos _khoaRepos;
        private readonly ChuyenNganhRepos _chuyenNganhRepos;
        private readonly NienKhoaRepos _nienKhoaRepos;
        private readonly PhongHocRepos _phongHocRepos;
        private readonly CanBoRepos _canBoRepos;
        private readonly ChucVuRepos _chucVuRepos;
        private readonly HocPhanRepos _hocPhanRepos;
        private readonly MonHocRepos _monHocRepos;

        public LopHocPhanController(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
            _lopHocPhanRepos = new LopHocPhanRepos(_connectionDatabase);
            _khoaRepos = new KhoaRepos(_connectionDatabase);
            _chuyenNganhRepos = new ChuyenNganhRepos(_connectionDatabase);
            _nienKhoaRepos = new NienKhoaRepos(_connectionDatabase);
            _phongHocRepos = new PhongHocRepos(_connectionDatabase);
            _canBoRepos = new CanBoRepos(_connectionDatabase);
            _chucVuRepos = new ChucVuRepos(_connectionDatabase);
            _hocPhanRepos = new HocPhanRepos(connectionDatabase);
            _monHocRepos = new MonHocRepos(connectionDatabase);
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
            var list = _nienKhoaRepos.GetAllNienKhoa();
            return Json(list);
        }

        [HttpGet]
        public IActionResult GetALLPhongHoc()
        {
            var list = _phongHocRepos.GetAllPhong();
            return Json(list);
        }

        [HttpGet]
        public IActionResult GetALLChucVu()
        {
            var list = _chucVuRepos.GetAllChucVu();
            return Json(list);
        }

        [HttpGet]
        public IActionResult GetChuyenNganhByKhoa(int maKhoa)
        {
            var list = _chuyenNganhRepos.GetChuyenNganhByKhoa(maKhoa);
            return Json(list);
        }

        [HttpGet]
        public IActionResult GetGiangVienByKhoa(int? maKhoa)
        {
            var list = _canBoRepos.GetCanBoFiltered(maKhoa);
            return Json(list);
        }

        [HttpGet]
        public IActionResult GetHocPhanByNienKhoa(string maNK)
        {
            var list = _hocPhanRepos.GetHocPhanByNienKhoa(maNK);
            return Json(list);
        }

        [HttpGet]
        public IActionResult GetMonHocByChuyenNganh(int? maNganh)
        {
            var list = _monHocRepos.GetMonHocByNganh(maNganh);
            return Json(list);
        }

                public IActionResult Index()
        {
            var lhp = _lopHocPhanRepos.GetAllLopHocPhan();
            return View(lhp);
        }

        public IActionResult ThemLopHocPhan()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemLopHocPhan(LopHocPhan lopHocPhan)
        {
            if (string.IsNullOrEmpty(lopHocPhan.MaHP)) {
                TempData["ErrorMessage"] = "Chưa chọn học phần";
                return View();
            } 
            
            if (string.IsNullOrEmpty(lopHocPhan.MaCB))
            {
                TempData["ErrorMessage"] = "Chưa chọn giảng viên";
                return View();
            }

            if (string.IsNullOrEmpty(lopHocPhan.MaMH))
            {
                TempData["ErrorMessage"] = "Chưa chọn môn học";
                return View();
            }

            bool isAdded = _lopHocPhanRepos.AddLopHocPhan(lopHocPhan);

            if (isAdded)
            {
                TempData["SuccessMessage"] = "Đã được thêm thành công!";
                return RedirectToAction("Index", "LopHocPhan");
            }
            else
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm";
            }

            return RedirectToAction("Index", "LopHocPhan");
        }

        [HttpGet]
        public IActionResult SuaLopHocPhan(string maLHP)
        {
            var list = _lopHocPhanRepos.GetLopHocPhanById(maLHP);
            if (list == null)
            {
                TempData["ErrorMessage"] = "Lớp học phần không tồn tại.";
                return RedirectToAction("Index");
            }
            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaLopHocPhan(LopHocPhan lopHocPhan)
        {
            if (string.IsNullOrEmpty(lopHocPhan.MaHP))
            {
                TempData["ErrorMessage"] = "Chưa chọn học phần";
                return View();
            }

            if (string.IsNullOrEmpty(lopHocPhan.MaCB))
            {
                TempData["ErrorMessage"] = "Chưa chọn giảng viên";
                return View();
            }

            if (string.IsNullOrEmpty(lopHocPhan.MaMH))
            {
                TempData["ErrorMessage"] = "Chưa chọn môn học";
                return View();
            }

            bool isAdded = _lopHocPhanRepos.UpdateLopHocPhan(lopHocPhan);

            if (isAdded)
            {
                TempData["SuccessMessage"] = "Đã được cập nhật thành công!";
                return RedirectToAction("Index", "LopHocPhan");
            }
            else
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật";
            }

            return RedirectToAction("Index", "LopHocPhan");
        }

        public IActionResult XoaLopHocPhan(string maLHP)
        {
            bool isDeleted = _lopHocPhanRepos.DeleteLopHocPhan(maLHP);

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
        public IActionResult TimKiem(string maNK = null, int? maKhoa = null, int? maNganh = null, string maHP = null, int? maPhong = null, string maMH = null, string maCB = null, string TuKhoa = null)
        {
            var lhp = _lopHocPhanRepos.GetLopHocPhanFiltered(maNK, maKhoa, maNganh, maHP, maPhong, maMH, maCB, TuKhoa);
            return View(lhp);
        }

        public IActionResult XemChiTietLopHocPhan(string maLHP)
        {
            var list = _lopHocPhanRepos.GetLopHocPhanById(maLHP);
            if (list == null)
            {
                TempData["ErrorMessage"] = "Lớp học phần không tồn tại.";
                return RedirectToAction("Index");
            }
            return View(list);
        }
    }
}
