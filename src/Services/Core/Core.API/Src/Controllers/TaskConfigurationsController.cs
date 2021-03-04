using System.Threading.Tasks;
using Core.API.View;
using Core.API.View.ViewExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Objects.TaskConfigurations;
using State;
using State.Commands.Configurations;
using State.Queries;

namespace Core.API.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class TaskConfigurationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaskConfigurationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<PageViewModel<TaskConfiguration>>> SelectTaskConfigurations()
        {
            var result = await _mediator.Send(new SelectQuery<TaskConfiguration>());

            return PageViewModel<TaskConfiguration>.Create(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskConfiguration>> FindTaskConfiguration(ulong id)
        {
            var result = await _mediator.Send(new FindQuery<TaskConfiguration>(id));

            return result.ToView();
        }
    }
}
