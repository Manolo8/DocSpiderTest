using Core.Domain.Documents.Models;
using Core.Storage.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Storage.Configurations.Documents {
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>, IMainContextConfiguration {
        public void Configure(EntityTypeBuilder<Document> builder) {
            builder.HasKey(e => e.Id);

            builder.HasIndex(e => e.Title).IsUnique();

            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Title).HasMaxLength(100);
            builder.Property(e => e.Description).HasMaxLength(2000);

            //o archive não é removido quando da delete no document
            //da pra criar um script para remover os archives posteriormente
            //e um sistema para recuperar arquivos...?
            builder.HasOne(e => e.Archive)
                   .WithMany()
                   .HasForeignKey(e => e.ArchiveId);

            builder.HasOne(e => e.User)
                   .WithMany(e => e.Documents)
                   .HasForeignKey(e => e.UserId);
        }
    }
}