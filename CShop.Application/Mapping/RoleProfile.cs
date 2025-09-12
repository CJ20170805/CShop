using AutoMapper;
using CShop.Application.DTOs;
using CShop.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Application.Mapping
{
    public class RoleProfile: Profile
    {
        public RoleProfile() 
        { 
           CreateMap<AppRole, RoleDto>().ReverseMap();
        }
    }
}
