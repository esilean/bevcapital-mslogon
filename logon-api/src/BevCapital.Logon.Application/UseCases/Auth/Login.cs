using BevCapital.Logon.Application.Errors;
using BevCapital.Logon.Application.Gateways.Security;
using BevCapital.Logon.Application.UseCases.Auth.Response;
using BevCapital.Logon.Application.Validators;
using BevCapital.Logon.Domain.Constants;
using BevCapital.Logon.Domain.Entities;
using BevCapital.Logon.Domain.Notifications;
using BevCapital.Logon.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace BevCapital.Logon.Application.UseCases.Auth
{
    public class Login
    {
        public class LoginCommand : IRequest<UserTokenOut>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class CommandValidator : AbstractValidator<LoginCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).Password();
            }
        }

        public class Handler : IRequestHandler<LoginCommand, UserTokenOut>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IAppNotificationHandler _appNotificationHandler;
            private readonly ITokenGenerator _tokenGenerator;
            private readonly IPasswordHasher<AppUser> _passwordHasher;

            public Handler(IUnitOfWork unitOfWork,
                           IAppNotificationHandler appNotificationHandler,
                           ITokenGenerator tokenGenerator,
                           IPasswordHasher<AppUser> passwordHasher)
            {
                _unitOfWork = unitOfWork;
                _appNotificationHandler = appNotificationHandler;
                _tokenGenerator = tokenGenerator;
                _passwordHasher = passwordHasher;
            }

            public async Task<UserTokenOut> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                var appUser = await _unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken);
                if (appUser == null)
                {
                    _appNotificationHandler.AddNotification(Keys.APPUSER, Messages.INVALID_EMAIL_PASSWORD);
                    return null;
                }

                var passwordResult = _passwordHasher.VerifyHashedPassword(appUser, appUser.Password, request.Password);
                if (passwordResult == PasswordVerificationResult.Failed)
                {
                    _appNotificationHandler.AddNotification(Keys.APPUSER, Messages.INVALID_EMAIL_PASSWORD);
                    return null;
                }

                var token = await _tokenGenerator.CreateToken(appUser);
                if (string.IsNullOrWhiteSpace(token))
                {
                    _appNotificationHandler.AddNotification(Keys.APPUSER, Messages.TOKEN_NOT_GENERATED);
                    return null;
                }

                return new UserTokenOut
                {
                    Id = appUser.Id,
                    Name = appUser.Name,
                    Email = appUser.Email,
                    Token = token
                };

                throw new AppException(HttpStatusCode.InternalServerError);
            }
        }
    }
}
