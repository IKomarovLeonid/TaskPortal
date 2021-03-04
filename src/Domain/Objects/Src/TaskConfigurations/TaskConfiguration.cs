using System;
using FluentValidation;
using FluentValidation.Results;
using Objects.Common;

namespace Objects.TaskConfigurations
{
    public class TaskConfiguration : IApplicationEntity
    {
        public ulong Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public EntityState State { get; set; }

        public Settings Settings { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }


        private static readonly IValidator<TaskConfiguration> Validation = new TaskConfigurationValidator();

        public ValidationResult Validate()
        {
            return Validation.Validate(this);
        }
    }
}
