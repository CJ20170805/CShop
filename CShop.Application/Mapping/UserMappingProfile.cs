using AutoMapper;
using CShop.Application.DTOs;
using CShop.Domain.Entities;
using CShop.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CShop.Application.Mapping
{
    public class UserMappingProfile: Profile
    {
        public UserMappingProfile()
        {
            // User mappings
            CreateMap<AppUser, UserDto>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore()) // roles are handled separately
                .ReverseMap();


            CreateMap<UserProfile, UserProfileDto>().ReverseMap();
            CreateMap<UserAddress, UserAddressDto>().ReverseMap();

        }
    }
}
