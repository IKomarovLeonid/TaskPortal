using FluentValidation;

namespace Objects.Servers
{
    class ServerValidator: AbstractValidator<Server>
    {
        public ServerValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(64);

            RuleFor(x => x.ConnectionSettings).NotNull().ChildRules(action =>
            {
                action.RuleFor(t => t.Address).NotEmpty()
                    .MinimumLength(0)
                    .MaximumLength(64);
                action.RuleFor(t => t.Login).NotEmpty()
                    .GreaterThan(ulong.MinValue)
                    .LessThan(ulong.MaxValue);
                action.RuleFor(t => t.Password).NotEmpty()
                    .MinimumLength(0)
                    .MaximumLength(64);
                action.RuleFor(t => t.Type)
                    .NotNull()
                    .NotEmpty()
                    .NotEqual(ConnectionType.NotDefined);
                
            });
        }
    }
}
