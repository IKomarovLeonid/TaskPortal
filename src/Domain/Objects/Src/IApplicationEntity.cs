using Objects.Common;

namespace Objects
{
    public interface IApplicationEntity
    {
        ulong Id { get; set; }

        EntityState State { get; set; }
    }
}
