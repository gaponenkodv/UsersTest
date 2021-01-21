using System;
using AppTest.Db.DTO;
using AppTest.Db.Models;
using AutoMapper;

namespace AppTest.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserRequest, User>();
            CreateMap<User, UserResponse>();
            CreateMap<Role, RoleResponse>();
        }
    }
}
