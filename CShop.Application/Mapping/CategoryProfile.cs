using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CShop.Domain.Entities;
using CShop.Application.DTOs;

namespace CShop.Application.Mapping
{
    public class CategoryProfile: Profile
    {
        public CategoryProfile()
        {
            // CreateMap<Source, Destination>();
            // Recursive mapping for Category to CategoryDto
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src => src.SubCategories))
                .ReverseMap();
        }
    }
}
