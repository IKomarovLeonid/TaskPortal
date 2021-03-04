using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brokeree.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Objects.ApplicationTasks;

namespace DataBase.DbConfigurations
{
    class GenerateTicksTasksDbConfiguration : IEntityTypeConfiguration<GenerateTicksTask>
    {
        public void Configure(EntityTypeBuilder<GenerateTicksTask> builder)
        {
            builder.ToTable("ticks_tasks");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id");

            builder.Property(t => t.ServerId).IsRequired().HasColumnName("server_id");
            builder.Property(t => t.ConfigurationId).IsRequired().HasColumnName("configuration_id");
            builder.Property(t => t.State).IsRequired().HasColumnName("state");
            builder.Property(t => t.Status).IsRequired().HasColumnName("status");
            builder.Property(t => t.Result).HasColumnName("result");
            builder.Property(t => t.Enabled).HasColumnName("activity");
            builder.Property(t => t.Count).HasColumnName("count");
            builder.Property(t => t.Symbols).HasColumnName("symbols");
            builder.Property(t => t.BidPrice).HasColumnName("bid");
            builder.Property(t => t.AskPrice).HasColumnName("ask");
            builder.Property(t => t.Spread).HasColumnName("spread");
            builder.Property(t => t.ProcessingTime).HasColumnName("process_time");
            builder.Property(t => t.CreatedTime).HasColumnName("created_time");
            builder.Property(t => t.UpdatedTime).HasColumnName("updated_time");
        }
    }
}
