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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=LoginPage}/{id?}");

            app.Run();
        }
    }
}