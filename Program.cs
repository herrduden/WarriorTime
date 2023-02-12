using Microsoft.EntityFrameworkCore;
using warriorTime.Models;

namespace warriorTime
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // equivalent de index.php
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = ".WarriorTime.Session";
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });
            string _connexionString=builder.Configuration.GetConnectionString("defaultMySQLCo");
            builder.Services.AddDbContext<WarriorTimeContext>(options => options.UseMySql(_connexionString, ServerVersion.AutoDetect(_connexionString)));
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
            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=LoginPage}/{id?}");

            app.Run();
        }
    }
}