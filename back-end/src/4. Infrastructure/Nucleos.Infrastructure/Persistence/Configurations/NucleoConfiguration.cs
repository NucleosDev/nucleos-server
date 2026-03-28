using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nucleos.Domain.Entities;

namespace Nucleos.Infrastructure.Persistence.Configurations;

public class NucleoConfiguration : IEntityTypeConfiguration<Nucleo>
{
    public void Configure(EntityTypeBuilder<Nucleo> entity)
    {
        entity.ToTable("nucleos");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnName("id");
        entity.Property(e => e.UserId).HasColumnName("user_id");
        entity.Property(e => e.Nome).HasColumnName("nome").IsRequired().HasMaxLength(100);
        entity.Property(e => e.Descricao).HasColumnName("descricao");
        entity.Property(e => e.Tipo).HasColumnName("tipo");
        entity.Property(e => e.CorDestaque).HasColumnName("cor_destaque");
        entity.Property(e => e.ImagemCapa).HasColumnName("imagem_capa");
        entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.HasOne(e => e.User).WithMany(u => u.Nucleos).HasForeignKey(e => e.UserId);
    }
}
