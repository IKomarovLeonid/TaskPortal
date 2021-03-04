using Brokeree.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Objects.Servers;

namespace DataBase.DbConfigurations
{
    class ServerDbConfiguration : IEntityTypeConfiguration<Server>
    {
        public void Configure(EntityTypeBuilder<Server> builder)
        {
            builder.ToTable("servers");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id");

            builder.Property(t => t.State).IsRequired().HasColumnName("state");
            builder.Property(t => t.Name).Tiny().IsRequired().HasColumnName("name");
            builder.Property(t => t.ConnectionSettings).IsObject().HasColumnName("settings");
            builder.Property(t => t.Enabled).HasColumnName("activity");
            builder.Property(t => t.CreateTime).HasColumnName("created_time");
            builder.Property(t => t.UpdateTime).HasColumnName("updated_time");
        }
    }
}
