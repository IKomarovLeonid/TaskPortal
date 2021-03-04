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
    public class GenerateAccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GenerateAccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<PageViewModel<AccountsTask>>> GetAll()
        {
            var result = await _mediator.Send(new SelectQuery<AccountsTask>());

            return PageViewModel<AccountsTask>.Create(result);
        }

        [HttpPost]
        public async Task<ActionResult<AffectionViewModel>> CreateTask([FromBody] CreateGenerateAccountsTaskRequestModel request)
        {
            var result = await _mediator.Send(new CreateGenerateAccountsTaskCommand()
            {
                ServerId = request.ServerId,
                ConfigurationId = request.ConfigurationId,
                Groups = request.Groups,
                AccountName = request.AccountName,
                AccountPassword = request.AccountPassword,
                Leverage = request.Leverage,
                MinBalance = request.MinBalance,
                MaxBalance = request.MaxBalance,
                MinCredit = request.MinCredit,
                MaxCredit = request.MaxCredit,
                Count = request.Count
            });

            return result.ToView();

        }
    }
}
