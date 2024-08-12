using GiyimSitesi.Context;
using GiyimSitesi.Context.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace GiyimSitesi.Services
{
    public static class StartupService
    {


        
		public static void AddService(this IServiceCollection services)
        {

			services.AddDbContext<Baglanti>(options => options.UseSqlServer(ConfigureService.ConnectionString));
			services.AddIdentity<Kullanici, IdentityRole>().AddEntityFrameworkStores<Baglanti>().AddDefaultTokenProviders();

			services.Configure<IdentityOptions>(options =>
			{
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequiredLength = 6;

				options.Lockout.MaxFailedAccessAttempts = 5;
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(1);
				options.Lockout.AllowedForNewUsers = true;

				options.User.RequireUniqueEmail = true;
				options.SignIn.RequireConfirmedEmail = false;
				options.SignIn.RequireConfirmedPhoneNumber = false;

			});

			services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/account/AccesDenied";
				options.LogoutPath = "/account/logout";
				options.AccessDeniedPath = "/account/accessdenied";
				options.SlidingExpiration = true;
				options.ExpireTimeSpan = TimeSpan.FromDays(1);
				options.Cookie = new CookieBuilder
				{
					HttpOnly = true,
					Name = ".Metin.Security.Cookie",
					SameSite = SameSiteMode.Strict
				};
			});



		}
    }
}
