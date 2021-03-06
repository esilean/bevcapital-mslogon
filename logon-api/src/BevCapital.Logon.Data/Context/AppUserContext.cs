using BevCapital.Logon.Data.Context.Configs;
using BevCapital.Logon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BevCapital.Logon.Data.Context
{
    public class AppUserContext : DbContext
    {
        public AppUserContext(DbContextOptions<AppUserContext> options)
                : base(options)
        { }

        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new AppUserEntityMapping());
        }
    }
}
