using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using IntegracaoPagador.Models;

namespace IntegracaoPagador.Mappers
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            {
                Mapper.Initialize(x =>
                {
                    x.AddProfile<SaleViewModelToRestSend>();
                    //x.AddProfile<ViewModelToDomainMappingProfile>();
                });
            }
        }
    }
}