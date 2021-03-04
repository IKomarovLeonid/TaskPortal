using Objects.Common;

namespace Objects.Settings
{
    public class JobSettings : IApplicationEntity
    {
        public string Name { get; set; }

        public JobTimers Timers { get; set; }

        // TODO remove
        public ulong Id { get; set; }
        public EntityState State { get; set; }
        //

        public JobSettings()
        {
            Name = "Jobs";
            Timers = new JobTimers();
        }


    }
}
