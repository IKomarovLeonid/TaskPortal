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
    class GeneratePositionsDbConfiguration : IEntityTypeConfiguration<GeneratePositionsTask>
    {
        public void Configure(EntityTypeBuilder<GeneratePositionsTask> builder)
        {
            builder.ToTable("positions_tasks");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id");

            builder.Property(t => t.ServerId).IsRequired().HasColumnName("server_id");
            builder.Property(t => t.ConfigurationId).IsRequired().HasColumnName("configuration_id");
            builder.Property(t => t.State).IsRequired().HasColumnName("state");
            builder.Property(t => t.Status).IsRequired().HasColumnName("status");
            builder.Property(t => t.Enabled).HasColumnName("activity");
            builder.Property(t => t.Count).HasColumnName("count");
            builder.Property(t => t.Direction).HasColumnName("direction");
            builder.Property(t => t.Groups).HasColumnName("groups");
            builder.Property(t => t.Symbols).HasColumnName("symbols");
            builder.Property(t => t.MinOpenPrice).HasColumnName("min_price");
            builder.Property(t => t.MaxOpenPrice).HasColumnName("max_price");
            builder.Property(t => t.MinLots).HasColumnName("min_lots");
            builder.Property(t => t.MaxLots).HasColumnName("max_lots");
            builder.Property(t => t.TakeProfit).HasColumnName("take_profit");
            builder.Property(t => t.StopLoss).HasColumnName("stop_loss");
            builder.Property(t => t.CreatedTime).HasColumnName("created_time");
            builder.Property(t => t.UpdatedTime).HasColumnName("updated_time");
        }
    }
}
