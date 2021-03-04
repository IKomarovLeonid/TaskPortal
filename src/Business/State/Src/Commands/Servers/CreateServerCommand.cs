using MediatR;
using Objects.Servers;
using State.Src;

namespace State.Commands.Servers
{
    public class CreateServerCommand : IRequest<OperationResult>
    {
        public string Name { get; set; }

        public ConnectionSettings Settings { get; set; }
    }
}
