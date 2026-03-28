using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nucleos.Domain.Entities;

namespace Nucleos.Infrastructure.Persistence.Configurations;

public class ItemListaConfiguration : IEntityTypeConfiguration<ItemLista>
{
    public void Configure(EntityTypeBuilder<ItemLista> entity)
    {
        entity.ToTable("itens_lista");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnName("id");
        entity.Property(e => e.ListaId).HasColumnName("lista_id");
        entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
        entity.Property(e => e.Nome).HasColumnName("nome").IsRequired();
        entity.Property(e => e.Quantidade).HasColumnName("quantidade");
        entity.Property(e => e.ValorUnitario).HasColumnName("valor_unitario");
        entity.Property(e => e.ValorTotal).HasColumnName("valor_total");
        entity.Property(e => e.Checked).HasColumnName("checked");
        entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.HasOne(e => e.Lista).WithMany(l => l.Itens).HasForeignKey(e => e.ListaId);
    }
}
