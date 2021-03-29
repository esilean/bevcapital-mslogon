using AutoMapper;
using BevCapital.Logon.Application.UseCases.User.Response;
using BevCapital.Logon.Domain.Entities;
using BevCapital.Logon.Domain.Events.AppUserEvents;
using System;

namespace BevCapital.Logon.Application.UseCases.User.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, AppUserOut<Guid>>();

            CreateMap<AppUser, AppUserCreatedEvent>();
            CreateMap<AppUser, AppUserUpdatedEvent>();
            CreateMap<AppUser, AppUserDeletedEvent>();
        }
    }
}
