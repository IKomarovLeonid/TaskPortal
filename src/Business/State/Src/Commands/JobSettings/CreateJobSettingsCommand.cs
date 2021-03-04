using MediatR;
using State.Src;

namespace State.Commands.JobSettings
{
    public class CreateJobSettingsCommand : IRequest<OperationResult>
    {
        public uint ReloadConnectionsTimer { get; } = 60;

        public uint ProcessTaskTimer { get; } = 60;

        public uint ProcessTaskResults { get; } = 60;
    }
}
