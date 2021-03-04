using System;
using FluentValidation;
using FluentValidation.Results;
using Objects.Common;
using Objects.Primitives;
using Objects.Results;
using Objects.Servers;

namespace Objects.ApplicationTasks
{
    public class AccountsTask : IApplicationEntity
    {
        public ulong Id { get; set; }

        public ulong ConfigurationId { get; set; }
        public ulong ServerId { get; set; }

        public ulong Count { get; set; }

        public EntityState State { get; set; }

        public TaskStatus Status { get; set; }

        public TaskResult Result { get; set; }

        public bool Enabled { get; set; }

        public string AccountName { get; set; } = string.Empty;

        public string AccountPassword { get; set; } = string.Empty;

        public string Groups { get; set; }

        public ulong Leverage { get; set; }

        public double MinBalance { get; set; }

        public double MaxBalance { get; set; }

        public double MinCredit { get; set; }

        public double MaxCredit { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }

        private static readonly IValidator<AccountsTask> Validation = new AccountTaskValidator();

        public ValidationResult Validate()
        {
            return Validation.Validate(this);
        }
    }
}
