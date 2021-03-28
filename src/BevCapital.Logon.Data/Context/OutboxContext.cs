using BevCapital.Logon.Domain.Core.Outbox;
using Microsoft.EntityFrameworkCore;

namespace BevCapital.Logon.Data.Context
{
    public class OutboxContext : DbContext
    {
        public OutboxContext(DbContextOptions<OutboxContext> options) : base(options)
        {
            if (!Database.CanConnect())
            {
                Database.EnsureCreated();
            }
        }

        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<OutboxMessage>(b =>
            {
                b.Property(p => p.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                b.Property(p => p.CreatedAtUtc)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                b.Property(p => p.Type)
                    .IsRequired();

                b.Property(p => p.Data)
                    .IsRequired();

                b.HasKey(k => k.Id);

                b.ToTable("Logon_OutboxMessages");
            });
        }
    }
}
