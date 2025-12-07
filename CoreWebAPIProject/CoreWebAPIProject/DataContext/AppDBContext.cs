using CoreWebAPIProject.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreWebAPIProject.DataContext
{
    public class AppDBContext:DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options):base(options) 
        { 
            
        }

        public DbSet<SaleMaster> saleMasters { get; set; }
        public DbSet<SaleDetail> saleDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SaleMaster>()
                .HasMany(s => s.saleDetail)
                .WithOne(d => d.saleMaster)
                .HasForeignKey(d => d.SaleId);


        }
    }
}
