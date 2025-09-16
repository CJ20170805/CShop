using AutoMapper;
using CShop.Domain.Entities;
using CShop.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CShop.Domain.ValueObjects;

namespace CShop.Application.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Price.Currency))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock.Quantity))
                .ReverseMap()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => new Money(src.Price, src.Currency)))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => new Stock(src.Stock)));


            CreateMap<ProductImage, ProductImageDto>().ReverseMap();
        }
    }
}
