using Artaxias.Data.Models.Organization;

using Microsoft.EntityFrameworkCore;

namespace Artaxias.Data.Configurations
{
    internal class ContractTemplateMappingConfiguration
    {
        internal static void Mapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContractTemplateMapping>().ToTable("ContractTemplateMapping");
            modelBuilder.Entity<ContractTemplateMapping>().Property(x => x.Id).IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            modelBuilder.Entity<ContractTemplateMapping>().HasKey(x => x.Id);
            modelBuilder.Entity<ContractTemplateMapping>().Property(x => x.TemplateField).IsRequired();
            modelBuilder.Entity<ContractTemplateMapping>().Property(x => x.EntityField);
            modelBuilder.Entity<ContractTemplateMapping>().HasOne(d => d.ContractTemplate).WithMany(u => u.Mappings).HasForeignKey(d => d.ContractTemplateId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
