using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using IntegracaoPagador.Models;
using IntegracaoPagador.SoapReference;

namespace IntegracaoPagador.Service
{
    public class SoapRequestService: ISoapRequestService
    {
        private readonly IPagadorSoapClientWrapper _pagadorTransactionWrapper;

        public SoapRequestService(IPagadorSoapClientWrapper pagadorTransaction)
        {
            _pagadorTransactionWrapper = pagadorTransaction;
        }
        public async Task<ResponseRequest> AuthorizeTransactionSoap(SaleViewModel sale, string merchantOrderId)
        {
            var soapSendRequestMessage = new AuthorizeTransactionRequest();
            soapSendRequestMessage.Version = "v1.0";
            soapSendRequestMessage.RequestId = Guid.NewGuid();
            soapSendRequestMessage.OrderData = new OrderDataRequest
            {
                MerchantId = new Guid(ConfigurationManager.AppSettings["merchantId"]),
                OrderId = merchantOrderId
            };

            soapSendRequestMessage.CustomerData = new CustomerDataRequest
            {
                CustomerName = sale.CustomerName
            };

            soapSendRequestMessage.PaymentDataCollection = new PaymentDataRequest[1];
            var payment = new CreditCardDataRequest
            {
                CardNumber = sale.CreditCardCardNumber,
                Amount = sale.PaymentAmount,
                CardExpirationDate = sale.CreditCardExpirationDate,
                CardHolder = sale.CreditCardHolder,
                CardSecurityCode = sale.CreditCardSecurityCode,
                PaymentMethod = 997,
                Currency = "BRL",
                Country = "BRA",
                NumberOfPayments = (short) (sale.PaymentInstallments),
                PaymentPlan = 0,
                TransactionType = 1
            };
            soapSendRequestMessage.PaymentDataCollection[0] = payment;
            
            var soapResponse =
                await _pagadorTransactionWrapper.AuthorizeTransactionAsync(soapSendRequestMessage);

            var responseObject = new ResponseRequest
            {
                PaymentId = soapResponse.PaymentDataCollection[0].BraspagTransactionId
            };

            return responseObject;
        }
    }
}