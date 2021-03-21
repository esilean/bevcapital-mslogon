using AutoMapper;
using BevCapital.Logon.Application.UseCases.User.Response;
using BevCapital.Logon.Domain.Entities;
using System;

namespace BevCapital.Logon.Application.UseCases.User.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, AppUserOut<Guid>>();
        }
    }
}
