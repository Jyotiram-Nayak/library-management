using library_management.Data.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace library_management.Data
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }
        public DbSet<Books> Books { get; set; }
        public DbSet<Authors> Authors { get; set; }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Books>()
        //        .HasOne(b => b.Authors)
        //        .WithMany(a => a.Books)
        //        .HasForeignKey(b => b.AuthorId);
        //}
    }   
}
