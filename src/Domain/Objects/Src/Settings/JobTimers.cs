using System;

namespace Objects.Settings
{
    public class JobTimers
    {
        public uint ProcessTaskResults { get; set; }
        public uint ReloadConnectionsTimer { get; set; }

        public uint ProcessTaskTimer { get; set; }

        public DateTime UpdatedUtc { get; set; }
    }
}
