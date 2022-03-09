using Artaxias.Data.Models.Organization;

using Microsoft.EntityFrameworkCore;

namespace Artaxias.Data.Configurations
{
    internal class BonusConfiguration
    {
        internal static void Mapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bonus>().ToTable("Bonus");
            modelBuilder.Entity<Bonus>().Property(x => x.Id).IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            modelBuilder.Entity<Bonus>().HasKey(x => x.Id);
            modelBuilder.Entity<Bonus>().Property(x => x.Amount).IsRequired();
            modelBuilder.Entity<Bonus>().Property(x => x.PaymentDate).IsRequired();
            modelBuilder.Entity<Bonus>().HasOne(d => d.Approver).WithMany(u => u.BonusesToApprove).HasForeignKey(d => d.ApproverId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Bonus>().HasOne(d => d.Requester).WithMany(u => u.BonusesRequested).HasForeignKey(d => d.RequesterId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Bonus>().HasOne(d => d.Receiver).WithMany(u => u.BonusesAssigned).HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Bonus>().HasOne(d => d.State).WithMany(t => t.Bonuses).HasForeignKey(d => d.DomainStateId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Bonus>().Property(x => x.CreatedDatetimeUTC)
                .HasColumnName(@"CreatedDatetimeUTC").HasColumnType(@"datetime2").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Bonus>().Property(x => x.UpdatedDatetimeUTC)
                .HasColumnName(@"UpdatedDatetimeUTC").HasColumnType(@"datetime2").ValueGeneratedOnUpdate().HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
