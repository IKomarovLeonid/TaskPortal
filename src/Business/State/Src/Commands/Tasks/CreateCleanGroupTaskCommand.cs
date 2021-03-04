using MediatR;
using State.Src;

namespace State.Commands.Tasks
{
    public class CreateCleanGroupTaskCommand : IRequest<OperationResult>
    {
        public ulong ServerId { get; set; }

        public ulong ConfigurationId { get; set; }

        public string Groups { get; set; }
    }
}
