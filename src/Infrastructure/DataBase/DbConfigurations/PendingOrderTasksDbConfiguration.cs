using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Objects.ApplicationTasks;

namespace DataBase.DbConfigurations
{
    class PendingOrderTasksDbConfiguration : IEntityTypeConfiguration<GeneratePendingOrdersTask>
    {
        public void Configure(EntityTypeBuilder<GeneratePendingOrdersTask> builder)
        {
            builder.ToTable("pending_orders_tasks");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id");

            builder.Property(t => t.ServerId).IsRequired().HasColumnName("server_id");
            builder.Property(t => t.ConfigurationId).IsRequired().HasColumnName("configuration_id");
            builder.Property(t => t.State).IsRequired().HasColumnName("state");
            builder.Property(t => t.Status).IsRequired().HasColumnName("status");
            builder.Property(t => t.Enabled).HasColumnName("activity");
            builder.Property(t => t.TotalCount).HasColumnName("total_count");
            builder.Property(t => t.PerAccountCount).HasColumnName("per_account_count");
            builder.Property(t => t.Type).HasColumnName("pending_type");
            builder.Property(t => t.PendingPrice).HasColumnName("price");
            builder.Property(t => t.Group).HasColumnName("group");
            builder.Property(t => t.CreatedTime).HasColumnName("created_time");
            builder.Property(t => t.UpdatedTime).HasColumnName("updated_time");
        }
    }
}
