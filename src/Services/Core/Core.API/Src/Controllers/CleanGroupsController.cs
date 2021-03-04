using System.Threading.Tasks;
using Core.API.View;
using Core.API.View.Tasks;
using Core.API.View.ViewExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Objects.ApplicationTasks;
using State.Commands.Tasks;
using State.Queries;

namespace Core.API.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class CleanGroupsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CleanGroupsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<PageViewModel<CleanGroupTask>>> GetAll()
        {
            var result = await _mediator.Send(new SelectQuery<CleanGroupTask>());

            return PageViewModel<CleanGroupTask>.Create(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CleanGroupTask>> GetById(uint id)
        {
            var result = await _mediator.Send(new FindQuery<CleanGroupTask>(id));

            return result.ToView();
        }

        [HttpPost]
        public async Task<ActionResult<AffectionViewModel>> CreateTask([FromBody] CreateCleanTaskRequestModel request)
        {
            var result = await _mediator.Send(new CreateCleanGroupTaskCommand()
            {
                ServerId = request.ServerId,
                ConfigurationId = request.ConfigurationId,
                Groups = request.Groups
            });
            
            return result.ToView();
        }


    }
}
