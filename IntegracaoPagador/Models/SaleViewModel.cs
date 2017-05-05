using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AutoMapper;
using IntegracaoPagador.Models.Enums;

namespace IntegracaoPagador.Models
{
    public class SaleViewModel
    {
        public string MerchantOrderId { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "The Customer Name  value cannot exceed 255 characters.")]
        public string CustomerName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The Payment type value cannot exceed 100 characters.")]
        public string PaymentType { get;  set; }
        [Required]
        public long PaymentAmount { get; set; }
        [Required]
        public virtual ProviderEnum PaymentProvider { get; set; }
        [Required]
        public int PaymentInstallments { get; set; }
        [Required]
        [StringLength(16, ErrorMessage = "The CreditCardCard Number value cannot exceed 16 characters.")]
        public string CreditCardCardNumber { get; set; }
        [Required]
        [StringLength(25, ErrorMessage = "The CreditCard Holder value cannot exceed 25 characters.")]
        public string CreditCardHolder { get; set; }
        [Required]
        [StringLength(7, ErrorMessage = "The CreditCard ExpirationDate value cannot exceed 7 characters.")]
        public string CreditCardExpirationDate { get; set; }
        [Required]
        [StringLength(4, ErrorMessage = "The CreditCard Security Code value cannot exceed 4 characters.")]
        public string CreditCardSecurityCode { get; set; }
        [Required]
        public BrandEnum CreditCardBrand { get; set; }
        [Required]
        public TypeSendEnum TypeSend { get; set; }

    }
}