using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegracaoPagador.Models
{
    public class RestSend
    {
        public string MerchantOrderId { get; set; }
        public Customer Customer { get; set; }
        public Payment Payment { get; set; }
        
    }
}