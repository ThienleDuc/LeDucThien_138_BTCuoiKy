using _224LTCs_LeDucThien_138.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// New add services to the container.
builder.Services.AddSingleton<ConnectionDatabase>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<CookieHelper>();
builder.Services.AddScoped<SinhVienRepos>();
builder.Services.AddScoped<CanBoRepos>();
builder.Services.AddScoped<LopSinhHoatRepos>();
builder.Services.AddScoped<KhoaRepos>();
builder.Services.AddScoped<ChuyenNganhRepos>();
builder.Services.AddScoped<NienKhoaRepos>();
builder.Services.AddScoped<PhongHocRepos>();
builder.Services.AddScoped<TaiKhoanAdminRepos>();
builder.Services.AddScoped<HocViRepos>();
builder.Services.AddScoped<ChucVuRepos>();
builder.Services.AddScoped<MonHocRepos>();
builder.Services.AddScoped<LopHocPhanRepos>();
builder.Services.AddScoped<CT_LHP_SVRepos>();
builder.Services.AddScoped<HocPhanRepos>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=LopHocPhan}/{action=Index}/{id?}");

app.Run();


