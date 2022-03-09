
using Artaxias.Data.Configurations;
using Artaxias.Data.Models;
using Artaxias.Data.Models.Feadback;
using Artaxias.Data.Models.Organization;
using Artaxias.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System.Linq;

namespace Artaxias
{

    public partial class ArtaxiasDbContext : IdentityDbContext<
            User, Role, int, IdentityUserClaim<int>, UserRole,
            IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {

        public ArtaxiasDbContext() :
            base()
        {
            OnCreated();
        }

        public ArtaxiasDbContext(DbContextOptions<ArtaxiasDbContext> options) :
            base(options)
        {
            OnCreated();
        }

        partial void CustomizeConfiguration(ref DbContextOptionsBuilder optionsBuilder);

        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Salary> Salaries { get; set; }
        public virtual DbSet<Absence> Vacations { get; set; }
        public virtual DbSet<FeedbackTemplate> FeadbackTemplates { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<AnswerOption> AnswerOptions { get; set; }
        public virtual DbSet<AnswerValue> AnswerValues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            PermissionMapping(modelBuilder);
            CustomizePermissionMapping(modelBuilder);

            RoleMapping(modelBuilder);
            CustomizeRoleMapping(modelBuilder);

            RolePermissionMapping(modelBuilder);
            CustomizeRolePermissionMapping(modelBuilder);

            UserMapping(modelBuilder);
            CustomizeUserMapping(modelBuilder);

            UserRoleMapping(modelBuilder);
            CustomizeUserRoleMapping(modelBuilder);

            RefreshTokenMapping(modelBuilder);
            SalaryMapping(modelBuilder);

            FeedbackAnswerValueMapping(modelBuilder);

            EmployeeConfiguration.Mapping(modelBuilder);
            DeparmentConfiguration.Mapping(modelBuilder);
            EmployeeDepartmentConfiguration.Mapping(modelBuilder);

            BonusConfiguration.Mapping(modelBuilder);
            AbsenceConfiguration.Mapping(modelBuilder);

            ContractTemplateConfiguration.Mapping(modelBuilder);
            ContractTemplateMappingConfiguration.Mapping(modelBuilder);

            ReviewDepartmentConfiguration.Mapping(modelBuilder);

            RelationshipsMapping(modelBuilder);

            CustomizeMapping(ref modelBuilder);
            modelBuilder.Entity<UserRole>().ToTable(nameof(UserRole));
        }

        #region Salary Mapping

        private void SalaryMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Salary>().ToTable("Salary");
            modelBuilder.Entity<Salary>().Property(x => x.Id).IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            modelBuilder.Entity<Salary>().HasKey(x => x.Id);
            modelBuilder.Entity<Salary>().Property(x => x.GrossAmount).IsRequired();
            modelBuilder.Entity<Salary>().Property(x => x.NetAmount).IsRequired();
            modelBuilder.Entity<Salary>().HasOne(d => d.Employee).WithMany(u => u.Salaries).HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        #endregion

        #region RefreshToken Mapping

        private void RefreshTokenMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RefreshToken>().Property(x => x.Id).IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            modelBuilder.Entity<RefreshToken>().Property(x => x.Created).IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<RefreshToken>().Property(x => x.Expires).IsRequired();
            modelBuilder.Entity<RefreshToken>().Property(x => x.Token).IsRequired();
        }

        #endregion

        #region Permission Mapping

        private void PermissionMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>().ToTable(@"Permission", @"dbo");
            modelBuilder.Entity<Permission>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            modelBuilder.Entity<Permission>().Property(x => x.Name).HasColumnName(@"Name").HasColumnType(@"varchar(256)").IsRequired().ValueGeneratedNever().HasMaxLength(256);
            modelBuilder.Entity<Permission>().Property(x => x.Code).HasColumnName(@"Code").HasColumnType(@"varchar(256)").IsRequired().ValueGeneratedNever().HasMaxLength(256);
            modelBuilder.Entity<Permission>().HasKey(@"Id");
        }

        partial void CustomizePermissionMapping(ModelBuilder modelBuilder);

        #endregion

        #region Role Mapping

        private void RoleMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().ToTable(@"Role", @"dbo");
            modelBuilder.Entity<Role>().Property(x => x.Id).HasColumnName(@"Id").HasColumnType(@"int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            modelBuilder.Entity<Role>().Property(x => x.Name).HasColumnName(@"Name").HasColumnType(@"varchar(256)").IsRequired().ValueGeneratedNever().HasMaxLength(256);
            modelBuilder.Entity<Role>().Property(x => x.CreatedByUserId).HasColumnName(@"CreatedByUserId").HasColumnType(@"int").ValueGeneratedNever();
            modelBuilder.Entity<Role>().Property(x => x.CreatedDatetimeUTC).HasColumnName(@"CreatedDatetimeUTC").HasColumnType(@"datetime2").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Role>().Property(x => x.UpdatedByUserId).HasColumnName(@"UpdatedByUserId").HasColumnType(@"int").ValueGeneratedNever();
            modelBuilder.Entity<Role>().Property(x => x.UpdatedDatetimeUTC).HasColumnName(@"UpdatedDatetimeUTC").HasColumnType(@"datetime2").ValueGeneratedOnUpdate().HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Role>().HasKey(@"Id");
        }

        partial void CustomizeRoleMapping(ModelBuilder modelBuilder);

        #endregion

        #region RolePermission Mapping

        private void RolePermissionMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolePermission>().ToTable(@"RolePermission", @"dbo");
            modelBuilder.Entity<RolePermission>().Property(x => x.RoleId).HasColumnName(@"RoleId").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<RolePermission>().Property(x => x.PermissionId).HasColumnName(@"PermissionId").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<RolePermission>().HasKey(x => new { x.RoleId, x.PermissionId });
        }

        partial void CustomizeRolePermissionMapping(ModelBuilder modelBuilder);

        #endregion

        #region User Mapping

        private void UserMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable(@"User", @"dbo");
            modelBuilder.Entity<User>().Property(x => x.FirstName).HasColumnName(@"FirstName").HasColumnType(@"varchar(256)").IsRequired().ValueGeneratedNever().HasMaxLength(256);
            modelBuilder.Entity<User>().Property(x => x.LastName).HasColumnName(@"LastName").HasColumnType(@"varchar(256)").IsRequired().ValueGeneratedNever().HasMaxLength(256);
            modelBuilder.Entity<User>().Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType(@"bit").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<User>().Property(x => x.CreatedDatetimeUTC).HasColumnName(@"CreatedDatetimeUTC").HasColumnType(@"datetime2").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<User>().Property(x => x.UpdatedDatetimeUTC).HasColumnName(@"UpdatedDatetimeUTC").HasColumnType(@"datetime2").ValueGeneratedOnUpdate().HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<User>().Property(x => x.HashingIterationCount).HasColumnName(@"HashingIterationCount").HasColumnType(@"int"); //ValueGeneratedOnAdd
            modelBuilder.Entity<User>().Property(x => x.HashingSalt).HasColumnName(@"HashingSalt").HasColumnType(@"binary(64)").ValueGeneratedNever();
        }

        partial void CustomizeUserMapping(ModelBuilder modelBuilder);

        #endregion

        #region UserRole Mapping

        private void UserRoleMapping(ModelBuilder modelBuilder)
        {
        }

        partial void CustomizeUserRoleMapping(ModelBuilder modelBuilder);

        #endregion

        #region FeedbackAnswerValue Mapping

        private void FeedbackAnswerValueMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FeedbackAnswerValue>().HasKey(x => new { x.AnswerValueId, x.FeedbackAnswerId });
        }

        #endregion

        private void RelationshipsMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>().HasMany(x => x.RolePermissions).WithOne(op => op.Permission).IsRequired(true).HasForeignKey(@"PermissionId");

            modelBuilder.Entity<Role>().HasOne(x => x.User_CreatedByUserId).WithMany(op => op.Roles_CreatedByUserId).IsRequired(false).HasForeignKey(@"CreatedByUserId");
            modelBuilder.Entity<Role>().HasOne(x => x.User_UpdatedByUserId).WithMany(op => op.Roles_UpdatedByUserId).IsRequired(false).HasForeignKey(@"UpdatedByUserId");
            modelBuilder.Entity<Role>().HasMany(x => x.RolePermissions).WithOne(op => op.Role).IsRequired(true).HasForeignKey(x => x.RoleId);
            modelBuilder.Entity<Role>().HasMany(x => x.UserRoles).WithOne(op => op.Role).IsRequired(true).HasForeignKey(@"RoleId");

            modelBuilder.Entity<RolePermission>().HasOne(x => x.Role).WithMany(op => op.RolePermissions).IsRequired(true).HasForeignKey(@"RoleId");
            modelBuilder.Entity<RolePermission>().HasOne(x => x.Permission).WithMany(op => op.RolePermissions).IsRequired(true).HasForeignKey(@"PermissionId");

            modelBuilder.Entity<User>().HasMany(x => x.Roles_CreatedByUserId).WithOne(op => op.User_CreatedByUserId).IsRequired(false).HasForeignKey(@"CreatedByUserId");
            modelBuilder.Entity<User>().HasMany(x => x.Roles_UpdatedByUserId).WithOne(op => op.User_UpdatedByUserId).IsRequired(false).HasForeignKey(@"UpdatedByUserId");
            modelBuilder.Entity<User>().HasMany(x => x.UserRoles).WithOne(op => op.User).IsRequired(true).HasForeignKey(@"UserId");

            modelBuilder.Entity<UserRole>().HasOne(x => x.User).WithMany(op => op.UserRoles).IsRequired(true).HasForeignKey(@"UserId");
            modelBuilder.Entity<UserRole>().HasOne(x => x.Role).WithMany(op => op.UserRoles).IsRequired(true).HasForeignKey(@"RoleId");

            modelBuilder.Entity<User>().HasMany(x => x.RefreshTokens).WithOne(o => o.User).HasForeignKey(f => f.UserId).IsRequired();
        }

        partial void CustomizeMapping(ref ModelBuilder modelBuilder);

        public bool HasChanges()
        {
            return ChangeTracker.Entries().Any(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);
        }

        partial void OnCreated();
    }
}