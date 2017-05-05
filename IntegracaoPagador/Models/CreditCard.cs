using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IntegracaoPagador.Models.Enums;

namespace IntegracaoPagador.Models
{
    public class CreditCard
    {
        public string CardNumber { get; set; }

        public string Holder { get; set; }
        public string ExpirationDate { get; set; }

        public string SecurityCode { get; set; }
       
        public BrandEnum Brand { get; set; }

      
    }
}