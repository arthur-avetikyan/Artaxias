using Artaxias.Data.Models.Organization;

using Microsoft.EntityFrameworkCore;

namespace Artaxias.Data.Configurations
{
    internal class AbsenceConfiguration
    {
        internal static void Mapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Absence>().ToTable("Absence");
            modelBuilder.Entity<Absence>().Property(x => x.Id).IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            modelBuilder.Entity<Absence>().HasKey(x => x.Id);
            modelBuilder.Entity<Absence>().Property(x => x.Reason).IsRequired(false);
            modelBuilder.Entity<Absence>().Property(x => x.StartDate).IsRequired();
            modelBuilder.Entity<Absence>().Property(x => x.EndDate).IsRequired();
            modelBuilder.Entity<Absence>().HasOne(d => d.Approver).WithMany(u => u.AbsencesToApprove).HasForeignKey(d => d.ApproverId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Absence>().HasOne(d => d.Receiver).WithMany(u => u.AbsencesAssigned).HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Absence>().HasOne(d => d.Type).WithMany(t => t.AbsencesAssigned).HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Absence>().HasOne(d => d.State).WithMany(t => t.Absences).HasForeignKey(d => d.DomainStateId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Absence>().Property(x => x.CreatedDatetimeUTC)
                .HasColumnName(@"CreatedDatetimeUTC").HasColumnType(@"datetime2").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Absence>().Property(x => x.UpdatedDatetimeUTC)
                .HasColumnName(@"UpdatedDatetimeUTC").HasColumnType(@"datetime2").ValueGeneratedOnUpdate().HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
