using MediatR;
using Objects;

namespace State.Queries
{
    public class FindQuery <TModel> : IRequest<FindResult<TModel>>
    {
        public FindQuery(ulong id)
        {
            Id = id;
        }

        public ulong Id { get; }
    }
}
