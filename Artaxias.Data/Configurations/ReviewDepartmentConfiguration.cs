using Artaxias.Data.Models.Feadback;

using Microsoft.EntityFrameworkCore;

namespace Artaxias.Data.Configurations
{
    internal class ReviewDepartmentConfiguration
    {
        internal static void Mapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReviewDepartment>().ToTable("ReviewDepartment");
            modelBuilder.Entity<ReviewDepartment>().Property(x => x.ReviewId).HasColumnName(@"ReviewId").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ReviewDepartment>().Property(x => x.DepartmentId).HasColumnName(@"DepartmentId").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ReviewDepartment>().HasKey(x => new { x.ReviewId, x.DepartmentId });

            modelBuilder.Entity<ReviewDepartment>().HasOne(x => x.Review).WithMany(op => op.Departments)
                .IsRequired(true).HasForeignKey(x => x.ReviewId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ReviewDepartment>().HasOne(x => x.Department).WithMany(op => op.Reviews)
                .IsRequired(true).HasForeignKey(x => x.DepartmentId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}