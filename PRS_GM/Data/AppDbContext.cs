using Microsoft.EntityFrameworkCore;
using PRS_GM.Models;

namespace PRS_GM.Data {
    public class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) {
        }
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Vendor> Vendors { get; set; } = default!;
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Request> Requests { get; set; } = default!;
    }
}
