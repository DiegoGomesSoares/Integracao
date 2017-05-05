using IntegracaoPagador.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegracaoPagador.Models
{
    public class ResponseRequest
    {
        [Required]
        public Guid PaymentId { get; set; }

        [Required]
        public TypeSendEnum TypeSend { get; set; }
    }
}