using FluentValidation;

namespace Objects.TaskConfigurations
{
    class TaskConfigurationValidator: AbstractValidator<TaskConfiguration>
    {
        public TaskConfigurationValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(64);
        }
    }
}
