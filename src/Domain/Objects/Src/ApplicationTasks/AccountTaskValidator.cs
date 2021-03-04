using FluentValidation;

namespace Objects.ApplicationTasks
{
    class AccountTaskValidator : AbstractValidator<AccountsTask>
    {
        public AccountTaskValidator()
        {
            RuleFor(t => t.Count)
                .GreaterThan(ulong.MinValue)
                .LessThan(ulong.MaxValue);

            RuleFor(t => t.Leverage)
                .GreaterThan(ulong.MinValue)
                .LessThan(ulong.MaxValue);
            RuleFor(t => t.AccountName)
                .NotNull()
                .MinimumLength(6)
                .MaximumLength(64);
            RuleFor(t => t.AccountPassword)
                .NotNull()
                .MinimumLength(6)
                .MaximumLength(32);
            RuleFor(t => t.Groups)
                .NotNull()
                .NotEmpty();
            RuleFor(t => t.MinBalance)
                .NotNull()
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(t => t.MaxBalance);
            RuleFor(t => t.MaxBalance)
                .NotNull()
                .GreaterThanOrEqualTo(t => t.MinBalance)
                .LessThan(double.MaxValue);
            RuleFor(t => t.MinCredit)
                .NotNull()
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(t => t.MaxCredit);
            RuleFor(t => t.MaxCredit)
                .NotNull()
                .GreaterThanOrEqualTo(t => t.MinCredit)
                .LessThan(double.MaxValue);
        }
    }
}
