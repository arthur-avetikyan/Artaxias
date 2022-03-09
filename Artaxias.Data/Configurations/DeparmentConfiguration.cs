using Artaxias.Data.Models.Organization;

using Microsoft.EntityFrameworkCore;

namespace Artaxias.Data.Configurations
{
    internal class DeparmentConfiguration
    {
        internal static void Mapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().ToTable("Department");
            modelBuilder.Entity<Department>().Property(x => x.Id).IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            modelBuilder.Entity<Department>().HasKey(x => x.Id);
            modelBuilder.Entity<Department>().Property(x => x.Name).IsRequired();
        }
    }
}
