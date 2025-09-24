using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CShop.Application.DTOs;
using CShop.Domain.Entities;
using CShop.Domain.ValueObjects;

namespace CShop.Application.Mapping
{
    public class CartProfile: Profile
    {
        public CartProfile()
        {
            // Cart mapping
            CreateMap<Cart, CartDto>().ReverseMap();

            // CartItem mapping
            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice.Amount))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.UnitPrice.Currency))
                .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.SubTotal.Amount))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product.ProductImages.FirstOrDefault() != null ? src.Product.ProductImages.FirstOrDefault()!.ImageUrl : null))
                .ReverseMap()
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => new Money(src.UnitPrice, src.Currency)));
        }
    }
}
