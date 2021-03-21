using AutoMapper;
using BevCapital.Logon.Application.UseCases.User.Response;
using BevCapital.Logon.Domain.Constants;
using BevCapital.Logon.Domain.Notifications;
using BevCapital.Logon.Domain.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BevCapital.Logon.Application.UseCases.User
{
    public class Details
    {
        public class Query : IRequest<AppUserOut<Guid>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, AppUserOut<Guid>>
        {

            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private readonly IAppNotificationHandler _appNotificationHandler;

            public Handler(IUnitOfWork unitOfWork, IMapper mapper, IAppNotificationHandler appNotificationHandler)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _appNotificationHandler = appNotificationHandler;
            }

            public async Task<AppUserOut<Guid>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _unitOfWork.Users.FindAsync(request.Id);
                if (user == null)
                {
                    _appNotificationHandler.AddNotification(Keys.APPUSER, Messages.USER_NOT_FOUND);
                    return null;
                }

                return _mapper.Map<AppUserOut<Guid>>(user);
            }
        }
    }
}
