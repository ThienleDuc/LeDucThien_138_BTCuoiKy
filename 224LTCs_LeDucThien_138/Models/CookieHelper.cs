namespace _224LTCs_LeDucThien_138.Models
{
    public class CookieHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Lưu tài khoản và mật khẩu vào cookie
        public void SetUserCredentials(string username, string password)
        {
            // Thiết lập cookie cho tài khoản và mật khẩu
            _httpContextAccessor.HttpContext.Response.Cookies.Append("UserName", username, new CookieOptions
            {
                Expires = DateTime.Now.AddMonths(1), // Thời gian sống của cookie
                HttpOnly = true, // Bảo mật cookie (không thể truy cập từ JavaScript)
                Secure = true // Chỉ gửi qua HTTPS
            });

            _httpContextAccessor.HttpContext.Response.Cookies.Append("Password", password, new CookieOptions
            {
                Expires = DateTime.Now.AddMonths(1),
                HttpOnly = true,
                Secure = true
            });
        }

        // Lấy tài khoản và mật khẩu từ cookie
        public (string username, string password) GetUserCredentials()
        {
            var username = _httpContextAccessor.HttpContext.Request.Cookies["UserName"];
            var password = _httpContextAccessor.HttpContext.Request.Cookies["Password"];

            return (username, password);
        }

        // Xóa cookie khi người dùng đăng xuất
        public void RemoveUserCredentials()
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("UserName");
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("Password");
        }

        // Hàm SetCookie: lưu giá trị vào cookie
        public void SetCookie(string key, string value, int? expireTimeInMinutes = null)
        {
            CookieOptions option = new CookieOptions();

            if (expireTimeInMinutes.HasValue)
                option.Expires = DateTime.Now.AddMonths(expireTimeInMinutes.Value);
            else
                option.Expires = DateTime.Now.AddMonths(1);

            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, option);
        }

        // Hàm DeleteCookie: xóa cookie theo tên
        public void RemoveCookie(string key)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
        }

        // Hàm GetCookie (nếu cần lấy ra nữa)
        public string GetCookie(string key)
        {
            return _httpContextAccessor.HttpContext.Request.Cookies[key];
        }
    }
}
