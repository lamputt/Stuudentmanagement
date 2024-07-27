using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudentManagement.Data;

namespace StudentManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure services
            builder.Services.AddDbContext<StudentManagementContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("StudentManagementContext") ?? throw new InvalidOperationException("Connection string 'StudentManagementContext' not found.")));

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Accounts/Login"; // Đường dẫn trang đăng nhập
                    options.AccessDeniedPath = "/Accounts/AccessDenied"; // Đường dẫn trang không có quyền truy cập
                    options.LogoutPath = "/Accounts/Logout"; // Đường dẫn trang đăng xuất
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = context =>
                        {
                            // Nếu người dùng chưa đăng nhập, chuyển hướng đến trang đăng nhập
                            context.Response.Redirect(context.RedirectUri);
                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddAuthorization();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure middleware
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication(); // Thêm middleware xác thực
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Accounts}/{action=Login}/{id?}"); // Đặt trang Login là mặc định

            app.Run();
        }
    }
}
