using BevCapital.Logon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BevCapital.Logon.Data.Context.Configs
{
    public class AppUserEntityMapping : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("Logon_AppUsers");
            builder.HasKey(r => r.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.CreatedAtUtc)
                .IsRequired();
            builder.Property(e => e.UpdatedAtUtc)
                .IsRequired();
            builder.Property(e => e.RowVersion)
                .IsRowVersion();

            builder.Ignore(e => e.Valid);
            builder.Ignore(e => e.Invalid);
            builder.Ignore(e => e.ValidationResult);
        }
    }
}
