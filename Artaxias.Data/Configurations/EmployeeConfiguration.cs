using Artaxias.Data.Models.Organization;

using Microsoft.EntityFrameworkCore;

namespace Artaxias.Data.Configurations
{
    internal class EmployeeConfiguration
    {
        internal static void Mapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("Employee");
            modelBuilder.Entity<Employee>().Property(x => x.Id).IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            modelBuilder.Entity<Employee>().HasKey(x => x.Id);
            modelBuilder.Entity<Employee>().Property(x => x.Position).IsRequired();
            modelBuilder.Entity<Employee>().Property(x => x.ContractStart).IsRequired();
            modelBuilder.Entity<Employee>().HasMany(x => x.Salaries).WithOne(x => x.Employee).HasForeignKey(f => f.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Employee>().HasOne(d => d.User).WithOne(u => u.Employee).HasForeignKey<Employee>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
