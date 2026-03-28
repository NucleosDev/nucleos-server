using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nucleos.Domain.Entities;

namespace Nucleos.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.HasIndex(u => u.Email)
            .IsUnique();
        
        builder.Property(u => u.Cpf)
            .IsRequired()
            .HasMaxLength(14);
        
        builder.HasIndex(u => u.Cpf)
            .IsUnique();
        
        builder.Property(u => u.PasswordHash)
            .IsRequired();
        
        builder.Property(u => u.Phone)
            .HasMaxLength(20);
        
        builder.Property(u => u.Active)
            .IsRequired()
            .HasDefaultValue(true);
        
        builder.Property(u => u.EmailVerified)
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.Property(u => u.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");
        
        builder.Property(u => u.UpdatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");
        
        // Relacionamentos
        builder.HasOne(u => u.Profile)
            .WithOne(p => p.User)
            .HasForeignKey<UserProfile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(u => u.Security)
            .WithOne(s => s.User)
            .HasForeignKey<UserSecurity>(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(u => u.Preference)
            .WithOne(p => p.User)
            .HasForeignKey<UserPreference>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(u => u.Level)
            .WithOne(l => l.User)
            .HasForeignKey<UserLevel>(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(u => u.Roles)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(u => u.Nucleos)
            .WithOne(n => n.User)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(u => u.Notifications)
            .WithOne(n => n.User)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(u => u.Streaks)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(u => u.Conquistas)
            .WithOne(uc => uc.User)
            .HasForeignKey(uc => uc.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(u => u.XpLogs)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(u => u.EnergyLogs)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(u => u.ActivityLogs)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(u => u.AiInteractions)
            .WithOne(ai => ai.User)
            .HasForeignKey(ai => ai.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(u => u.AiContext)
            .WithOne(ac => ac.User)
            .HasForeignKey<AIContext>(ac => ac.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(u => u.Subscription)
            .WithOne(s => s.User)
            .HasForeignKey<Subscription>(s => s.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}