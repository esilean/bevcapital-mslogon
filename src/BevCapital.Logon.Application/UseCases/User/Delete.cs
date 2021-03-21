using BevCapital.Logon.Application.Errors;
using BevCapital.Logon.Domain.Constants;
using BevCapital.Logon.Domain.Notifications;
using BevCapital.Logon.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace BevCapital.Logon.Application.UseCases.User
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IAppNotificationHandler _appNotificationHandler;
            private readonly IDistributedCache _distributedCache;

            public Handler(IUnitOfWork unitOfWork,
                           IAppNotificationHandler appNotificationHandler,
                           IDistributedCache distributedCache)
            {
                _unitOfWork = unitOfWork;
                _appNotificationHandler = appNotificationHandler;
                _distributedCache = distributedCache;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var appUser = await _unitOfWork.Users.FindAsync(request.Id);
                if (appUser == null)
                {
                    _appNotificationHandler.AddNotification(Keys.APPUSER, Messages.USER_NOT_FOUND);
                    return Unit.Value;
                }

                _unitOfWork.Users.Remove(appUser);

                var success = await _unitOfWork.SaveAsync();
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
