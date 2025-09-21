using AutoMapper;
using CShop.Application.DTOs;
using CShop.Domain.Entities;
using CShop.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Application.Mapping
{
    public class OrderProfile: Profile
    {
        public OrderProfile()
        {
            // Order mappings
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.RecipientName, opt => opt.MapFrom(src => src.ShippingAddress.RecipientName))
                .ForMember(dest => dest.AddressLine1, opt => opt.MapFrom(src => src.ShippingAddress.AddressLine1))
                .ForMember(dest => dest.AddressLine2, opt => opt.MapFrom(src => src.ShippingAddress.AddressLine2))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.ShippingAddress.City))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.ShippingAddress.State))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.ShippingAddress.PostalCode))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.ShippingAddress.Country))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount.Amount))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.TotalAmount.Currency));

            CreateMap<OrderDto, Order>()
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => new ShippingAddress(
                    src.RecipientName,
                    src.AddressLine1,
                    src.AddressLine2,
                    src.City,
                    src.State,
                    src.PostalCode,
                    src.Country
                )))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => new Money(src.TotalAmount, src.Currency)));

            // OrderItem mappings
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice.Amount))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.UnitPrice.Currency))
                .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.SubTotal.Amount));

            CreateMap<OrderItemDto, OrderItem>()
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => new Money(src.UnitPrice, src.Currency)))
                .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => new Money(src.SubTotal, src.Currency)));
        }
    }
}
