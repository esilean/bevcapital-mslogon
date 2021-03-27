using BevCapital.Logon.Domain.Core.Events;
using FluentValidation;
using System;

namespace BevCapital.Logon.Domain.Events.AppUserEvents
{
    public class AppUserCreatedEvent : Event
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public class Validator : AbstractValidator<AppUserCreatedEvent>
        {
            public Validator()
            {
                RuleFor(e => e.UserId).NotEmpty();
                RuleFor(e => e.Name).NotEmpty();
                RuleFor(e => e.Email).NotEmpty().EmailAddress();
            }
        }
    }
}
