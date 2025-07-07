using Microsoft.EntityFrameworkCore;
using project2025.Models;

namespace project2025.DBContexts
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<LearnPost> Learn { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Access> Accesses { get; set; }

        public DbSet<RequestList> Requests { get; set; }

        public DbSet<xogame_table> Xogames { get; set; }

        public DbSet<Fortess> fortesses { get; set; }

    }
}
