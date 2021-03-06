using AutoMapper;
using BevCapital.Logon.Application.Errors;
using BevCapital.Logon.Domain.Constants;
using BevCapital.Logon.Domain.Entities;
using BevCapital.Logon.Domain.Events.AppUserEvents;
using BevCapital.Logon.Domain.Notifications;
using BevCapital.Logon.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace BevCapital.Logon.Application.UseCases.User
{
    public class Delete
    {
        public class DeleteAppUserCommand : IRequest
        {
            public string Email { get; set; }
        }

        public class Handler : IRequestHandler<DeleteAppUserCommand>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IAppNotificationHandler _appNotificationHandler;
            private readonly IDistributedCache _distributedCache;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork unitOfWork,
                           IAppNotificationHandler appNotificationHandler,
                           IDistributedCache distributedCache,
                           IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _appNotificationHandler = appNotificationHandler;
                _distributedCache = distributedCache;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(DeleteAppUserCommand request, CancellationToken cancellationToken)
            {
                var appUser = await _unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken);
                if (appUser == null)
                {
                    _appNotificationHandler.AddNotification(Keys.APPUSER, Messages.USER_NOT_FOUND);
                    return Unit.Value;
                }

                await _unitOfWork.Users.RemoveAsync(appUser, cancellationToken);

                var @event = _mapper.Map<AppUser, AppUserDeletedEvent>(appUser);
                @event.UserId = appUser.Email;

                var success = await _unitOfWork.SaveAndCommitEvents(@event);
                if (success)
                {
                    await _distributedCache.RemoveAsync(CacheKeys.LIST_ALL_USERS);
                    return Unit.Value;
                }

                throw new AppException(HttpStatusCode.InternalServerError);
            }
        }
    }
}
