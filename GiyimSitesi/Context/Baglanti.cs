using GiyimSitesi.Context.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GiyimSitesi.Context
{
	public class Baglanti : IdentityDbContext<Kullanici>
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//optionsBuilder.UseSqlServer("server=77.245.159.10;database=GunesGiyimDB;uid=drnserhat;pwd=Flayer.053215; TrustServerCertificate=true;");
			optionsBuilder.UseSqlServer("server=DESKTOP-BEJ32B9;database=Giyim3;integrated security=true;TrustServerCertificate=true;");
		}
		public Baglanti()
        {
				
        }
        public Baglanti(DbContextOptions<Baglanti> options)
			: base(options)
		{ }

		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Contact> Contacts { get; set; }
	}


}
