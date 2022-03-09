using Artaxias.Data.Models.Organization;

using Microsoft.EntityFrameworkCore;

namespace Artaxias.Data.Configurations
{
    internal class ContractTemplateConfiguration
    {
        internal static void Mapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContractTemplate>().ToTable("ContractTemplate");
            modelBuilder.Entity<ContractTemplate>().Property(x => x.Id).IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            modelBuilder.Entity<ContractTemplate>().HasKey(x => x.Id);
            modelBuilder.Entity<ContractTemplate>().Property(x => x.Title).IsRequired();
            modelBuilder.Entity<ContractTemplate>().HasOne(d => d.CreatedByUser).WithMany(u => u.ContractTemplates).HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ContractTemplate>().Property(x => x.CreatedDatetimeUTC)
                .HasColumnName(@"CreatedDatetimeUTC").HasColumnType(@"datetime2").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<ContractTemplate>().Property(x => x.UpdatedDatetimeUTC)
                .HasColumnName(@"UpdatedDatetimeUTC").HasColumnType(@"datetime2").ValueGeneratedOnUpdate().HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
