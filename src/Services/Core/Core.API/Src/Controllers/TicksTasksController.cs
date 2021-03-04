using System.Threading.Tasks;
using Core.API.View;
using Core.API.View.Tasks;
using Core.API.View.ViewExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using State.Commands.Tasks;

namespace Core.API.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class TicksTasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicksTasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<AffectionViewModel>> CreateTask([FromBody] CreateGenerateTicksTaskRequestModel request)
        {
            var result = await _mediator.Send(new CreateGenerateTicksTaskCommand()
            {
                ServerId = request.ServerId,
                ConfigurationId = request.ConfigurationId,
                Symbols = request.Symbols,
                Bid = request.Bid,
                Ask = request.Ask,
                Spread = request.Spread,
                Count = request.Count
            });

            return result.ToView();
        }
    }
}
