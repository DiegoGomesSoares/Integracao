using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using IntegracaoPagador.Models;

namespace IntegracaoPagador.Mappers
{
    public class SaleViewModelToRestSend : Profile
    {
        protected override void Configure()
        {

            Mapper.CreateMap<SaleViewModel, Payment>()
                .ForMember(d => d.Type, o => o.MapFrom(s => s.PaymentType))
                .ForMember(d => d.Amount, o => o.MapFrom(s => s.PaymentAmount))
                .ForMember(d => d.Installments, o => o.MapFrom(s => s.PaymentInstallments))
                .ForMember(d => d.Provider, o => o.MapFrom(s => s.PaymentProvider))
                .ForMember(d => d.CreditCard, o => o.Ignore())
                .ForMember(d => d.PaymentId, o => o.Ignore());
                
            Mapper.CreateMap<SaleViewModel, Customer>().ForMember(d => d.Name, o => o.MapFrom(s => s.CustomerName));

            Mapper.CreateMap<SaleViewModel, CreditCard>()
                .ForMember(d => d.CardNumber, o => o.MapFrom(s => s.CreditCardCardNumber))
                .ForMember(d => d.Brand, o => o.MapFrom(s => s.CreditCardBrand))
                .ForMember(d => d.SecurityCode, o => o.MapFrom(s => s.CreditCardSecurityCode))
                .ForMember(d => d.ExpirationDate, o => o.MapFrom(s => s.CreditCardExpirationDate))
                .ForMember(d => d.Holder, o => o.MapFrom(s => s.CreditCardHolder));

            Mapper.AssertConfigurationIsValid();
        }
    }
}