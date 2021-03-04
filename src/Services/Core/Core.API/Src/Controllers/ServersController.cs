using System.Threading.Tasks;
using Core.API.View;
using Core.API.View.Servers;
using Core.API.View.ViewExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Objects.Servers;
using State.Commands.Servers;
using State.Queries;

namespace Core.API.Controllers
{

    [ApiController, Route("api/[controller]")]
    public class ServersController : ControllerBase
    {
        private readonly IMediator _mediator;
    
        public ServersController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<ActionResult<PageViewModel<ServerModel>>> GetServers()
        {
            var result = await _mediator.Send(new SelectQuery<ServerModel>());

            return PageViewModel<ServerModel>.Create(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServerModel>> GetById(ulong id)
        {
            var result = await _mediator.Send(new FindQuery<ServerModel>(id));

            return result.ToView();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<AffectionViewModel>> Delete(ulong id)
        {
            var result = await _mediator.Send(new ArchiveServerCommand
            {
                Id = id
            });

            return result.ToView();
        }

        [HttpPost]
        public async Task<ActionResult<AffectionViewModel>> Create([FromBody] CreateServerRequestModel userRequest)
        {
            var result = await _mediator.Send(new CreateServerCommand
            {
                Name = userRequest.Name,
                Settings = userRequest.Settings
            });

            return result.ToView();
        }
    }
}
