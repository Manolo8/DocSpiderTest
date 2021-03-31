using Core.Domain.Archives.Models;
using Core.Storage.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Storage.Configurations.Archives {
    public class ArchiveConfiguration : IEntityTypeConfiguration<Archive>, IMainContextConfiguration {
        public void Configure(EntityTypeBuilder<Archive> builder) {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Name).HasMaxLength(128);
        }
    }
}