using System;

namespace Core.API.View
{
    public class AffectionViewModel
    {
        public ulong Id { get; }

        public DateTime AffectionTimeUtc { get; }

        public AffectionViewModel(ulong id)
        {
            Id = id;
            AffectionTimeUtc = DateTime.UtcNow;
        }
    }
}
