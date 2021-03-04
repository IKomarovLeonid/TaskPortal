using Brokeree.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Objects.Servers;
using Objects.TaskConfigurations;

namespace DataBase.DbConfigurations
{
    class ConfigurationsBbConfiguration : IEntityTypeConfiguration<TaskConfiguration>
    {
        public void Configure(EntityTypeBuilder<TaskConfiguration> builder)
        {
            builder.ToTable("task_configurations");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id");

            builder.Property(t => t.Name).IsRequired().HasColumnName("name");
            builder.Property(t => t.IsActive).IsRequired().HasColumnName("is_active");
            builder.Property(t => t.State).IsRequired().HasColumnName("state");
            builder.Property(t => t.Settings).IsObject().HasColumnName("settings");
            builder.Property(t => t.CreatedTime).HasColumnName("created_time");
            builder.Property(t => t.UpdatedTime).HasColumnName("updated_time");
        }
    }
}
