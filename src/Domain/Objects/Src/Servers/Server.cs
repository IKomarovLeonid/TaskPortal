using System;
using FluentValidation;
using FluentValidation.Results;
using Objects.Common;

namespace Objects.Servers
{
    public class Server : IApplicationEntity
    {
        public ulong Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public bool Enabled { get; set; }

        public EntityState State { get; set; }
        
        public ConnectionSettings ConnectionSettings { get; set; } = new ConnectionSettings();

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        private static readonly IValidator<Server> Validation = new ServerValidator();

        public ValidationResult Validate()
        {
            return Validation.Validate(this);
        }
    }
}
