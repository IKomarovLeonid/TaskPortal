using MediatR;
using State.Src;

namespace State.Commands.Servers
{
    public class ArchiveServerCommand : IRequest<OperationResult>
    {
        public ulong Id { get; set; }
    }
}
