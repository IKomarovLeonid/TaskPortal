using System;
using MetaTrader4;
using Objects.Common;

namespace Objects.Servers
{
    public class ServerModel
    {
        public string Name { get; set; }

        public ulong Id { get; set; }

        public EntityState State { get; set; }

        public bool IsActive { get; set; }

        public ConnectionSettings Settings { get; set; }

        public ConnectionInfo ConnectionInfo { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
