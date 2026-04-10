using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nucleos.Domain.Entities;

namespace Nucleos.Infrastructure.Persistence.Configurations;

public class BlocoConfiguration : IEntityTypeConfiguration<Bloco>
{
    public void Configure(EntityTypeBuilder<Bloco> entity)
    {
        entity.ToTable("blocos");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnName("id");
        entity.Property(e => e.NucleoId).HasColumnName("nucleo_id");
        entity.Property(e => e.Tipo).HasColumnName("tipo").IsRequired();
        entity.Property(e => e.Titulo).HasColumnName("titulo").IsRequired();
        entity.Property(e => e.Posicao).HasColumnName("posicao");
        

        entity.Property(e => e.Configuracoes)
            .HasColumnName("configuracoes")
            .HasColumnType("jsonb")
            .HasDefaultValueSql("'{}'::jsonb");
            
        entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        
        entity.HasOne(e => e.Nucleo)
            .WithMany(n => n.Blocos)
            .HasForeignKey(e => e.NucleoId)
            .OnDelete(DeleteBehavior.Cascade); // ✅ Se deletar núcleo, deleta blocos
    }
}