using System;
using MediatR;
using State.Commands.JobSettings;
using State.Src;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Objects.Settings;
using Processing.Abstract;
using Processing.Repository;

namespace State.Handlers.JobSettings
{
    class CreateJobSettingsHandler : IRequestHandler<CreateJobSettingsCommand, OperationResult>
    {
        // services
        private readonly IRepositoryManager _manager;

        public CreateJobSettingsHandler(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public async Task<OperationResult> Handle(CreateJobSettingsCommand request, CancellationToken cancellationToken)
        {
            var repository = _manager.Resolve<Objects.Settings.JobSettings>();

            var settings = await repository.GetAllAsync();

            if(settings.Any()) return OperationResult.Applied();

            var jobs = new Objects.Settings.JobSettings();

            // fill data
            jobs.Timers.ProcessTaskResults = request.ProcessTaskResults;
            jobs.Timers.ProcessTaskTimer = request.ProcessTaskTimer;
            jobs.Timers.ReloadConnectionsTimer = request.ReloadConnectionsTimer;
            jobs.Timers.UpdatedUtc = DateTime.UtcNow;
            // save 
            await repository.AddAsync(jobs);

            return OperationResult.Applied();
        }
    }
}
