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

        [HttpGet]
        public IActionResult TimKiem(string maNK = null, int? maKhoa = null, int? maNganh = null, string maHP = null, int? maPhong = null, int? maMH = null, string maCB = null, string TuKhoa = null)
        {
            var lhp = _lopHocPhanRepos.GetLopHocPhanFiltered(maNK, maKhoa, maNganh, maHP, maPhong, maMH, maCB, TuKhoa);
            return View(lhp);
        }
    }
}
