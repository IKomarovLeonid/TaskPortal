using Brokeree.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Objects.Settings;
using Objects.TaskConfigurations;

namespace DataBase.DbConfigurations
{
    class SettingsDbConfiguration : IEntityTypeConfiguration<JobSettings>
    {
        public void Configure(EntityTypeBuilder<JobSettings> builder)
        {
            builder.ToTable("settings");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id");

            builder.Property(t => t.State).IsRequired().HasColumnName("state");
            builder.Property(t => t.Name).IsRequired().HasColumnName("name");
            builder.Property(t => t.Timers).IsObject().HasColumnName("jobs");
        }
    }
}
