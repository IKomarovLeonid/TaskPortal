using System.Security.RightsManagement;
using MediatR;
using State.Src;

namespace State.Commands.Tasks
{
    public class CreateGenerateAccountsTaskCommand : IRequest<OperationResult>
    {
        public ulong ServerId { get; set; }
        public ulong ConfigurationId { get; set; }

        public ulong Count { get; set; }

        public string Groups { get; set; }

        public string AccountName { get; set; }

        public string AccountPassword { get; set; }

        public ulong Leverage { get; set; }

        public double MinBalance { get; set; }

        public double MaxBalance { get; set; }

        public double MinCredit { get; set; }

        public double MaxCredit { get; set; }
    }
}
