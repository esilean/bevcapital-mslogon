using AutoMapper;
using BevCapital.Logon.Application.UseCases.User.Response;
using BevCapital.Logon.Domain.Constants;
using BevCapital.Logon.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BevCapital.Logon.Application.UseCases.User
{
    public class List
    {
        public class Query : IRequest<List<AppUserOut<Guid>>> { }

        public class Handler : IRequestHandler<Query, List<AppUserOut<Guid>>>
        {

            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private readonly IDistributedCache _distributedCache;

            public Handler(IUnitOfWork unitOfWork, IMapper mapper, IDistributedCache distributedCache)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _distributedCache = distributedCache;
            }

            public async Task<List<AppUserOut<Guid>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var cachedAppUsers = await _distributedCache.GetAsync<List<AppUserOut<Guid>>>(CacheKeys.LIST_ALL_USERS);
                if (cachedAppUsers == null || !cachedAppUsers.Any())
                {
                    var appUsers = await _unitOfWork.Users.GetAllAsync();
                    cachedAppUsers = _mapper.Map<List<AppUserOut<Guid>>>(appUsers);

                    await _distributedCache.SetAsync<List<AppUserOut<Guid>>>
                                                    (
                                                        CacheKeys.LIST_ALL_USERS,
                                                        cachedAppUsers,
                                                        new DistributedCacheEntryOptions
                                                        {
                                                            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                                                        }
                                                    );
                }

                return cachedAppUsers;
            }
        }
    }
}
