using System;
using Objects.Common;
using Objects.Servers;
using Objects.Src.Servers;

namespace View.Src.Models
{
    public class ServerViewModel
    {
        public ulong Id { get; set; }

        public string Name { get; set; }

        public EntityState State { get; set; }

        public bool IsActive { get; set; }

        public ConnectionSettings Settings { get; set; }

        public ConnectionInfo ConnectionInfo { get; set; }

        public DateTime CreatedTimeUtc { get; set; }

        public DateTime UpdateTimeUtc { get; set; }
    }
}
