using DataBase.DbConfigurations;
using Microsoft.EntityFrameworkCore;
using Objects.ApplicationTasks;
using Objects.Results;
using Objects.Servers;
using Objects.TaskConfigurations;
using JobSettings = Objects.Settings.JobSettings;

namespace DataBase
{
    public class DataContext : DbContext
    {
        public DbSet<Server> Servers { get; set; }

        public DbSet<AccountsTask> AccountsTasks { get; set; }

        public DbSet<CleanGroupTask> CleanGroupTasks { get; set; }

        public DbSet<GeneratePendingOrdersTask> PendingOrdersTasks { get; set; }

        public DbSet<GeneratePositionsTask> PositionsTasks { get; set; }

        public DbSet<GenerateTicksTask> TicksTasks { get; set; }

        public DbSet<TaskConfiguration> TaskConfigurations { get; set; }

        public DbSet<GenerateResultInfo> GenerateResults { get; set; }

        public DbSet<JobSettings> JobsSettings { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AccountTaskDbConfiguration());
            modelBuilder.ApplyConfiguration(new ClearGroupTaskDbConfiguration());
            modelBuilder.ApplyConfiguration(new ConfigurationsBbConfiguration());
            modelBuilder.ApplyConfiguration(new GeneratePositionsDbConfiguration());
            modelBuilder.ApplyConfiguration(new GenerateTicksTasksDbConfiguration());
            modelBuilder.ApplyConfiguration(new PendingOrderTasksDbConfiguration());
            modelBuilder.ApplyConfiguration(new ServerDbConfiguration());
            modelBuilder.ApplyConfiguration(new SettingsDbConfiguration());
            modelBuilder.ApplyConfiguration(new GenerateResultsInfoDbConfiguration());

            modelBuilder.Entity<AccountsTask>()
                .HasOne<Server>()
                .WithMany()
                .HasForeignKey(t => t.ServerId)
                .HasForeignKey(t => t.ConfigurationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CleanGroupTask>()
                .HasOne<Server>()
                .WithMany()
                .HasForeignKey(t => t.ServerId)
                .HasForeignKey(t => t.ConfigurationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GeneratePositionsTask>()
                .HasOne<Server>()
                .WithMany()
                .HasForeignKey(t => t.ServerId)
                .HasForeignKey(t => t.ConfigurationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GenerateTicksTask>()
                .HasOne<Server>()
                .WithMany()
                .HasForeignKey(t => t.ServerId)
                .HasForeignKey(t => t.ConfigurationId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<GeneratePendingOrdersTask>()
                .HasOne<Server>()
                .WithMany()
                .HasForeignKey(t => t.ServerId)
                .HasForeignKey(t => t.ConfigurationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GenerateResultInfo>();

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
        }
    }
}
