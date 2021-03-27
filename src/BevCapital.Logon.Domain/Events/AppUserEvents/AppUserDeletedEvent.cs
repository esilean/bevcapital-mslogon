using BevCapital.Logon.Domain.Core.Events;
using FluentValidation;
using System;

namespace BevCapital.Logon.Domain.Events.AppUserEvents
{
    public class AppUserDeletedEvent : Event
    {
        public Guid UserId { get; set; }

        public class Validator : AbstractValidator<AppUserDeletedEvent>
        {
            public Validator()
            {
                RuleFor(e => e.UserId).NotEmpty();
            }
        }
    }
}
