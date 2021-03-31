using Core.Domain.Users.Models;
using Core.Storage.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Storage.Configurations.Users {
    public class UserConfiguration : IEntityTypeConfiguration<User>, IMainContextConfiguration {
        public void Configure(EntityTypeBuilder<User> builder) {
            builder.HasKey(e => e.Id);

            builder.HasIndex(e => e.Email).IsUnique();

            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Name).HasMaxLength(128);
            builder.Property(e => e.Email).HasMaxLength(128);
            builder.Property(e => e.Active);
            builder.Property(e => e.Verified);
            builder.Property(e => e.Password).HasMaxLength(48);
            builder.Property(e => e.LastModifiedDate);
        }
    }
}