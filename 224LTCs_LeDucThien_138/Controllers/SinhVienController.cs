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
        private readonly HocPhanRepos _hocPhanRepos;
        private readonly CT_LHP_SVRepos _ctLHP_SVRepos;
        private readonly LopHocPhanRepos _lopHocPhanRepos;

        public SinhVienController(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
            _sinhVienRepos = new SinhVienRepos(_connectionDatabase);
            _khoaRepos = new KhoaRepos(_connectionDatabase);
            _chuyenNganhRepos = new ChuyenNganhRepos(_connectionDatabase);
            _nienKhoaRepos = new NienKhoaRepos(_connectionDatabase);
            _lopSinhHoatRepos = new LopSinhHoatRepos(_connectionDatabase);
            _hocPhanRepos = new HocPhanRepos(_connectionDatabase);
            _ctLHP_SVRepos = new CT_LHP_SVRepos(_connectionDatabase);
            _lopHocPhanRepos = new LopHocPhanRepos(connectionDatabase);
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
                return RedirectToAction("SinhVien", "Login");
            }
            return View(sv);
        }

        public IActionResult XemThoiKhoaBieu()
        {
            string maSV = "22010222101";

            var sv = _sinhVienRepos.GetSinhVienById(maSV);

            if (sv == null)
            {
                TempData["ErrorMessage"] = "Sinh viên không tồn tại.";
                return RedirectToAction("SinhVien", "Login");
            }

            return View(sv);
        }

        [Authorize(Roles = "SinhVien")]
        public IActionResult DangKyLophocPhan()
        {
            var sv = _sinhVienRepos.GetSinhVienById(User.Identity.Name);

            if (sv == null)
            {
                TempData["ErrorMessage"] = "Sinh viên không tồn tại.";
                return RedirectToAction("SinhVien", "Login");
            }

            string maHP = _hocPhanRepos.GetMaHPMoiNhat();
            ViewBag.MaHP = maHP;
            
            return View(sv);
        }

        [Authorize(Roles = "SinhVien")]
        [HttpPost]
        public IActionResult XacNhanDangKy(List<string> MaLHPs)
        {
            if (MaLHPs == null || !MaLHPs.Any())
            {
                TempData["ErrorMessage"] = "Bạn chưa chọn lớp nào.";
                return RedirectToAction("DangKyLopHocPhan");
            }

            // Lấy thông tin lớp học phần từ MaLHPs
            List<LopHocPhan> danhSachLop = new List<LopHocPhan>();
            foreach(var maLHP in MaLHPs)
            {
                var lop = _lopHocPhanRepos.GetLopHocPhanById(maLHP);
                if (lop != null)
                {
                    danhSachLop.Add(lop);
                }
            }

            string maHP = _hocPhanRepos.GetMaHPMoiNhat();
            ViewBag.MaHP = maHP;

            return View("XacNhanDangKy", new SinhVien
            {
                DanhSachLopHocPhanDaChon = danhSachLop
            });
        }

        [Authorize(Roles = "SinhVien")]
        [HttpPost]
        public IActionResult DangKyLopHocPhan(List<string> MaLHPs)
        {
            string MaSV = User.Identity.Name;
            var errors = new List<string>();
            bool tatCaThanhCong = true;

            foreach (var maLHP in MaLHPs)
            {
                bool ketQua = _ctLHP_SVRepos.SinhVienRegisterLopHocPhan(MaSV, maLHP, null, errors);
                if (!ketQua)
                {
                    tatCaThanhCong = false;
                }
            }

            if (tatCaThanhCong)
            {
                TempData["MaLHPs"] = string.Join(",", MaLHPs);
                return RedirectToAction("ThongBaoSauDangKy");
            }
            else
            {
                TempData["DanhSachLoi"] = string.Join("|||", errors);
                return RedirectToAction("ThongBaoSauDangKy");
            }
        }

        [Authorize(Roles = "SinhVien")]
        public IActionResult ThongBaoSauDangKy()
        {
            string maSV = User.Identity.Name;

            var sv = _sinhVienRepos.GetSinhVienById(maSV);

            if (sv == null)
            {
                TempData["ErrorMessage"] = "Sinh viên không tồn tại.";
                return RedirectToAction("SinhVien", "Login");
            }

            string maHP = _hocPhanRepos.GetMaHPMoiNhat();
            ViewBag.MaHP = maHP;

            // Lấy danh sách MaLHPs đã đăng ký từ TempData (nếu có)
            if (TempData["MaLHPs"] != null)
            {
                var maLHPs = TempData["MaLHPs"].ToString().Split(',').ToList();
                ViewBag.MaLHPsDaDangKy = maLHPs;
            }

            if (TempData["DanhSachLoi"] != null)
            {
                ViewBag.DanhSachLoi = TempData["DanhSachLoi"].ToString().Split("|||").ToList();
            }

            return View(sv);
        }

        [Authorize(Roles = "SinhVien")]
        [HttpPost]
        public IActionResult HuyDangKyLopHocPhan(List<string> MaLHPs)
        {
            string MaSV = User.Identity.Name;

            if (string.IsNullOrEmpty(MaSV) || MaLHPs == null || MaLHPs.Count == 0)
            {
                TempData["Error"] = "Vui lòng chọn ít nhất một lớp học phần để hủy.";
                return RedirectToAction("DangKyLophocPhan");
            }

            foreach (var maLHP in MaLHPs)
            {
                _ctLHP_SVRepos.DeleteLopHocPhanOfSinhVien(MaSV, maLHP);
            }

            TempData["Message"] = "Đã hủy thành công các lớp học phần đã chọn.";
            return RedirectToAction("DangKyLophocPhan");
        }
    }
}
