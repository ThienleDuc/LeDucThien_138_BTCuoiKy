using _224LTCs_LeDucThien_138.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// New add services to the container.
builder.Services.AddSingleton<ConnectionDatabase>();

builder.Services.AddScoped<PhongHocRepos>();
builder.Services.AddScoped<CanBoRepos>();
builder.Services.AddScoped<KhoaRepos>();
builder.Services.AddScoped<ChuyenNganhRepos>();
builder.Services.AddScoped<HocViRepos>();
builder.Services.AddScoped<ChucVuRepos>();
builder.Services.AddScoped<SinhVienRepos>();
builder.Services.AddScoped<LopSinhHoatRepos>();
builder.Services.AddScoped<NienKhoaRepos>();
builder.Services.AddScoped<MonHocRepos>();
builder.Services.AddScoped<LopHocPhanRepos>();
builder.Services.AddScoped<CT_LHP_SVRepos>();
builder.Services.AddScoped<HocPhanRepos>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<TaiKhoanAdminRepos>();

builder.Services.AddAuthentication(options =>
{
    // Dùng policy scheme làm mặc định
    options.DefaultScheme = "DualScheme";
    options.DefaultChallengeScheme = "DualScheme";
    options.DefaultSignInScheme = "DualScheme";
})
// Policy scheme này “chuyển tiếp” sang AdminScheme hoặc SinhVienScheme
.AddPolicyScheme("DualScheme", "Dynamic scheme", options =>
{
    options.ForwardDefaultSelector = context =>
    {
        // Nếu request có cookie AdminAuthCookie, chọn AdminScheme
        if (context.Request.Cookies.ContainsKey("AdminAuthCookie"))
            return "AdminScheme";
        // Nếu request có cookie SinhVienAuthCookie, chọn SinhVienScheme
        if (context.Request.Cookies.ContainsKey("SinhVienAuthCookie"))
            return "SinhVienScheme";
        // Ngược lại, dùng scheme theo challenge (login)
        var path = context.Request.Path;
        if (path.StartsWithSegments("/Login/Admin", StringComparison.OrdinalIgnoreCase))
            return "AdminScheme";
        if (path.StartsWithSegments("/Login/SinhVien", StringComparison.OrdinalIgnoreCase))
            return "SinhVienScheme";
        // Mặc định rẽ về SinhVienScheme (hoặc AdminScheme tuỳ bạn)
        return "AdminScheme";
    };
})
.AddCookie("AdminScheme", o =>
{
    o.LoginPath = "/Login/Admin";
    o.Cookie.Name = "AdminAuthCookie";
    o.ExpireTimeSpan = TimeSpan.FromDays(30);
})
.AddCookie("SinhVienScheme", o =>
{
    o.LoginPath = "/Login/SinhVien";
    o.Cookie.Name = "SinhVienAuthCookie";
    o.ExpireTimeSpan = TimeSpan.FromDays(30);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


