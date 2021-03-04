using Brokeree.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Objects.ApplicationTasks;

namespace DataBase.DbConfigurations
{
    class AccountTaskDbConfiguration : IEntityTypeConfiguration<AccountsTask>
    {
        public void Configure(EntityTypeBuilder<AccountsTask> builder)
        {
            builder.ToTable("accounts_tasks");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id");

            builder.Property(t => t.ServerId).IsRequired().HasColumnName("server_id");
            builder.Property(t => t.ConfigurationId).IsRequired().HasColumnName("configuration_id");
            builder.Property(t => t.State).IsRequired().HasColumnName("state");
            builder.Property(t => t.Status).IsRequired().HasColumnName("status");
            builder.Property(t => t.Result).HasColumnName("result");
            builder.Property(t => t.Enabled).HasColumnName("activity");
            builder.Property(t => t.Count).HasColumnName("count");
            builder.Property(t => t.AccountName).HasColumnName("account_name");
            builder.Property(t => t.AccountPassword).HasColumnName("password");
            builder.Property(t => t.Groups).HasColumnName("groups");
            builder.Property(t => t.Leverage).HasColumnName("leverage");
            builder.Property(t => t.MinBalance).HasColumnName("min_balance");
            builder.Property(t => t.MaxBalance).HasColumnName("max_balance");
            builder.Property(t => t.MinCredit).HasColumnName("min_credit");
            builder.Property(t => t.MaxCredit).HasColumnName("max_credit");
            builder.Property(t => t.CreatedTime).HasColumnName("created_time");
            builder.Property(t => t.UpdatedTime).HasColumnName("updated_time");
        }
    }
}
