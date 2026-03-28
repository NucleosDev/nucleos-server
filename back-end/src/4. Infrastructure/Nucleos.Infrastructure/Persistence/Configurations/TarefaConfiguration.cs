using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nucleos.Domain.Entities;

namespace Nucleos.Infrastructure.Persistence.Configurations;

public class TarefaConfiguration : IEntityTypeConfiguration<Tarefa>
{
    public void Configure(EntityTypeBuilder<Tarefa> entity)
    {
        entity.ToTable("tarefas");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnName("id");
        entity.Property(e => e.BlocoId).HasColumnName("bloco_id");
        entity.Property(e => e.Titulo).HasColumnName("titulo").IsRequired();
        entity.Property(e => e.Descricao).HasColumnName("descricao");
        entity.Property(e => e.Prioridade).HasColumnName("prioridade");
        entity.Property(e => e.Status).HasColumnName("status");
        entity.Property(e => e.DataVencimento).HasColumnName("data_vencimento");
        entity.Property(e => e.ConcluidaEm).HasColumnName("concluida_em");
        entity.Property(e => e.Posicao).HasColumnName("posicao");
        entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.HasOne(e => e.Bloco).WithMany(b => b.Tarefas).HasForeignKey(e => e.BlocoId);
    }
}
