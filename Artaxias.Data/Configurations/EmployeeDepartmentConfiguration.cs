using Artaxias.Data.Models.Organization;

using Microsoft.EntityFrameworkCore;

namespace Artaxias.Data.Configurations
{
    internal class EmployeeDepartmentConfiguration
    {
        internal static void Mapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeDepartment>().ToTable("EmployeeDepartment");
            modelBuilder.Entity<EmployeeDepartment>().Property(x => x.EmployeeId).HasColumnName(@"EmployeeId").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<EmployeeDepartment>().Property(x => x.DepartmentId).HasColumnName(@"DepartmentId").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<EmployeeDepartment>().HasKey(x => new { x.EmployeeId, x.DepartmentId });

            modelBuilder.Entity<EmployeeDepartment>().HasOne(x => x.Employee).WithMany(op => op.Departments)
                .IsRequired(true).HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<EmployeeDepartment>().HasOne(x => x.Department).WithMany(op => op.Staff)
                .IsRequired(true).HasForeignKey(x => x.DepartmentId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}