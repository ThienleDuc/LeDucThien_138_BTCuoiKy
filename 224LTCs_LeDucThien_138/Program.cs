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

builder.Services.AddAuthentication("AdminScheme")
    .AddCookie("AdminScheme", o =>
    {
        o.LoginPath = "/Login/Admin";
        o.Cookie.Name = "AdminAuthCookie";
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


