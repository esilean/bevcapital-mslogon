using AutoMapper;
using BevCapital.Logon.Application.Errors;
using BevCapital.Logon.Application.Validators;
using BevCapital.Logon.Domain.Constants;
using BevCapital.Logon.Domain.Entities;
using BevCapital.Logon.Domain.Events.AppUserEvents;
using BevCapital.Logon.Domain.Notifications;
using BevCapital.Logon.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace BevCapital.Logon.Application.UseCases.User
{
    public class Create
    {
        public class CreateAppUserCommand : IRequest
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class CommandValidator : AbstractValidator<CreateAppUserCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).Password();
            }
        }

        public class Handler : IRequestHandler<CreateAppUserCommand>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IPasswordHasher<AppUser> _passwordHasher;
            private readonly IAppNotificationHandler _appNotificationHandler;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork unitOfWork,
                           IPasswordHasher<AppUser> passwordHasher,
                           IAppNotificationHandler appNotificationHandler,
                           IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _passwordHasher = passwordHasher;
                _appNotificationHandler = appNotificationHandler;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(CreateAppUserCommand request, CancellationToken cancellationToken)
            {
                if (await _unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken) != null)
                {
                    _appNotificationHandler.AddNotification(Keys.APPUSER, Messages.EMAIL_EXISTS);
                    return Unit.Value;
                }

                var appUser = AppUser.Create(request.Name, request.Email);
                if (appUser.Invalid)
                {
                    _appNotificationHandler.AddNotifications(appUser.ValidationResult);
                    return Unit.Value;
                }

                var passwordHash = _passwordHasher.HashPassword(appUser, request.Password);
                appUser.UpdatePassword(passwordHash);

                await _unitOfWork.Users.AddAsync(appUser, cancellationToken);

                var @event = _mapper.Map<AppUser, AppUserCreatedEvent>(appUser);
                @event.UserId = appUser.Id;

                var success = await _unitOfWork.SaveChangesAndCommitAsync(@event);
                if (success) return Unit.Value;

                throw new AppException(HttpStatusCode.InternalServerError);
            }
        }
    }
}
