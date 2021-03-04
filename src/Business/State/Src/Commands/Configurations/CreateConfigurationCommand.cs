using MediatR;
using State.Src;

namespace State.Commands.Configurations
{
    public class CreateConfigurationCommand : IRequest<OperationResult>
    {
        public string Name { get; set; } = "Default configuration";
    }
}
