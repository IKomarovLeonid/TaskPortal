using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySql.Data.MySqlClient;
using Objects.Results;

namespace DataBase.DbConfigurations
{
    class GenerateResultsInfoDbConfiguration : IEntityTypeConfiguration<GenerateResultInfo>
    {
        public void Configure(EntityTypeBuilder<GenerateResultInfo> builder)
        {
            builder.ToTable("generate_results");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id");

            builder.Property(t => t.TaskType).IsRequired().HasColumnName("Task type");
            builder.Property(t => t.TaskId).IsRequired().HasColumnName("task_id");
            builder.Property(t => t.ErrorCount).HasColumnName("error_count");
            builder.Property(t => t.ProcessedCount).HasColumnName("processed_count");
            builder.Property(t => t.RequestedCount).HasColumnName("requested_count");
            builder.Property(t => t.UpdatedTime).HasColumnName("updated_time");
        }
    }
}
