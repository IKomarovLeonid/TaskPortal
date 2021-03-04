using System.Collections.Generic;
using MediatR;

namespace State.Queries
{
    public class SelectQuery<TModel> : IRequest<ICollection<TModel>>
    {

    }
}
