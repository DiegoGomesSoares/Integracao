using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using IntegracaoPagador.Models.Enums;

namespace IntegracaoPagador.Models
{
    public class Payment
    {
        public string PaymentId { get; set; }         
        public string Type { get;  set; }
       
        public long Amount { get; set; }
       
        public  ProviderEnum Provider { get; set; }

        public int Installments { get; set; }

        public CreditCard CreditCard { get; set; }

     
       
    }
}