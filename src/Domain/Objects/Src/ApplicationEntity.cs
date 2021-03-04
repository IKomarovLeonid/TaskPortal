using Objects.Common;

namespace Objects
{
    abstract class ApplicationEntity : IApplicationEntity
    {
        ulong IApplicationEntity.Id { get; set; }
        EntityState IApplicationEntity.State { get; set; }
    }
}
