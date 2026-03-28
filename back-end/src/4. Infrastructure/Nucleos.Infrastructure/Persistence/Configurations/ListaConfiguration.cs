using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nucleos.Domain.Entities;

namespace Nucleos.Infrastructure.Persistence.Configurations;

public class ListaConfiguration : IEntityTypeConfiguration<Lista>
{
    public void Configure(EntityTypeBuilder<Lista> entity)
    {
        entity.ToTable("listas");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnName("id");
        entity.Property(e => e.BlocoId).HasColumnName("bloco_id");
        entity.Property(e => e.Nome).HasColumnName("nome").IsRequired();
        entity.Property(e => e.TipoLista).HasColumnName("tipo_lista");
        entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.HasOne(e => e.Bloco).WithMany(b => b.Listas).HasForeignKey(e => e.BlocoId);
    }
}
