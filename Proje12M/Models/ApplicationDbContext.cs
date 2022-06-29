using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Reflection;

namespace Proje12M.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<STI>()
    .Property(p => p.Miktar)
     .HasColumnType("numeric(25,6)");

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<STI> STIs { get; set; }
        public DbSet<STK> STKs { get; set; }


    }
}
