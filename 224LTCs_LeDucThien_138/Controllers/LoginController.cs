using Microsoft.AspNetCore.Mvc;
using _224LTCs_LeDucThien_138.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
namespace _224LTCs_LeDucThien_138.Controllers
{
    public class LoginController : Controller
    {
        private readonly ConnectionDatabase _connectionDatabase;
        private readonly TaiKhoanAdminRepos _taiKhoanAdminRepos;
        private readonly CanBoRepos _canBoRepos;
        private readonly SinhVienRepos _sinhVienRepos;

        public LoginController(ConnectionDatabase connectionDatabase)
        {
            _connectionDatabase = connectionDatabase;
            _taiKhoanAdminRepos = new TaiKhoanAdminRepos(_connectionDatabase);
            _canBoRepos = new CanBoRepos(_connectionDatabase);
            _sinhVienRepos = new SinhVienRepos(_connectionDatabase);
        }

        public IActionResult Admin()
        {
            return View();
        }

        // Xử lý đăng nhập
        [HttpPost]
        public async Task<IActionResult> Admin(string username, string password, bool rememberMe)
        {
            // Kiểm tra đăng nhập
            var admin = _taiKhoanAdminRepos.GetTaiKhoanAdmin(username, password);

            if (admin == null)
            {
                // Đăng nhập thất bại
                ViewBag.ErrorMessage = "Tài khoản hoặc mật khẩu không đúng!";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("HoTen", admin.HoTen ?? "-")
            };

            var claimsIdentity = new ClaimsIdentity(claims, "AdminScheme");
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = rememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30)
            };

            await HttpContext.SignInAsync("AdminScheme", 
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        // Phương thức Logout cho Admin
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> AdminLogout()
        {
            await HttpContext.SignOutAsync("AdminScheme");
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

        [HttpPost]
        public async Task<IActionResult> SinhVien(string username, string password, bool rememberMe)
        {
            // Kiểm tra đăng nhập
            var sv = _sinhVienRepos.GetTaiKhoanSinhVien(username, password);

            if (sv == null)
            {
                // Đăng nhập thất bại
                ViewBag.ErrorMessage = "Tài khoản hoặc mật khẩu không đúng!";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "SinhVien"),
                new Claim("TenSV", sv.TenSV ?? "-")
            };

            var claimsIdentity = new ClaimsIdentity(claims, "SinhVienScheme");
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = rememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30)
            };

            await HttpContext.SignInAsync("SinhVienScheme",
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("XemThoiKhoaBieu", "SinhVien");
        }

        // Phương thức Logout cho Admin
        [Authorize(Roles = "SinhVien")]
        [HttpPost]
        public async Task<IActionResult> SinhVienLogout()
        {
            await HttpContext.SignOutAsync("SinhVienScheme");
            return RedirectToAction("Index", "Home");
        }
    }
}
