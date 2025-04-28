using Microsoft.AspNetCore.Mvc;
using _224LTCs_LeDucThien_138.Models;
namespace _224LTCs_LeDucThien_138.Controllers
{
    public class LoginController : Controller
    {
        private readonly ConnectionDatabase _connectionDatabase;
        private readonly TaiKhoanAdminRepos _taiKhoanAdminRepos;
        private readonly GiangVienRepos _giangVienRepos;
        private readonly SinhVienRepos _sinhVienRepos;
        private readonly CookieHelper _cookieHelper;

        public LoginController(ConnectionDatabase connectionDatabase, CookieHelper cookieHelper)
        {
            _connectionDatabase = connectionDatabase;
            _taiKhoanAdminRepos = new TaiKhoanAdminRepos(_connectionDatabase);
            _giangVienRepos = new GiangVienRepos(_connectionDatabase);
            _sinhVienRepos = new SinhVienRepos(_connectionDatabase);
            _cookieHelper = cookieHelper;
        }

        public IActionResult Admin()
        {
            return View();
        }

        // Xử lý đăng nhập
        [HttpPost]
        public IActionResult Admin(string username, string password, bool rememberMe)
        {
            // Kiểm tra đăng nhập
            var admin = _taiKhoanAdminRepos.GetAllAdmin().FirstOrDefault(a => a.MaTaiKhoan == username && a.MatKhau == password);

            if (admin != null)
            {
                // Đăng nhập thành công, lưu thông tin vào cookie
                // Lưu tài khoản và mật khẩu
                if (rememberMe)
                {
                    _cookieHelper.SetUserCredentials(username, password);
                }
                // Lưu UserType
                _cookieHelper.SetCookie("UserType", "Admin");
                _cookieHelper.SetCookie("HoTen", admin.HoTen);
                _cookieHelper.SetCookie("MaTaiKhoan", admin.MaTaiKhoan);

                return RedirectToAction("Index", "Home"); // Redirect đến trang chủ hoặc trang Admin
            }
            else
            {
                // Đăng nhập thất bại
                ViewBag.ErrorMessage = "Tài khoản hoặc mật khẩu không đúng!";
                return View();
            }
        }

        // Phương thức Logout cho Admin
        [HttpPost]
        public IActionResult Logout()
        {
            // Lấy tài khoản và mật khẩu từ cookie
            var (username, password) = _cookieHelper.GetUserCredentials();

            // Kiểm tra nếu có tài khoản và mật khẩu trong cookie, tiến hành xóa
            if (username != null && password != null)
            {
                _cookieHelper.RemoveUserCredentials();
            }

            // Xoá cookie nếu có
            _cookieHelper.RemoveCookie("UserType");
            _cookieHelper.RemoveCookie("HoTen");
            _cookieHelper.RemoveCookie("MaTaiKhoan");

            // Redirect về trang đăng nhập hoặc trang chủ
            return RedirectToAction("Index", "Home");
        }


        public IActionResult GiangVien()
        {
            return View();
        }

        public IActionResult SinhVien()
        {
            return View();
        }
    }
}
