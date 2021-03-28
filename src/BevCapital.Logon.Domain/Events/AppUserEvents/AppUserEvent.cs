using BevCapital.Logon.Domain.Core.Events;
using FluentValidation;
using System;

namespace BevCapital.Logon.Domain.Events.AppUserEvents
{
    public class AppUserEvent : IEvent
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public class Validator : AbstractValidator<AppUserEvent>
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
