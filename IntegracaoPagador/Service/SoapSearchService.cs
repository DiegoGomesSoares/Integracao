using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using IntegracaoPagador.Models;
using IntegracaoPagador.Models.Enums;
using IntegracaoPagador.PagadorSoapSearch;

namespace IntegracaoPagador.Service
{
    public class SoapSearchService : ISoapSearchService
    {
        public readonly IPagadorSoapSearchClientWrapper PagadorSeachWrapper;

        public SoapSearchService(IPagadorSoapSearchClientWrapper pagadorSearch)
        {
            PagadorSeachWrapper = pagadorSearch;
        }

        public async Task<ResponseObject> Search(ResponseViewModel sale)
        {
            var responseObject = new ResponseObject();

            var transactionDataRequest = MountTransactionDataRequest(sale);

            var creditCardRequest = MountCreditCardDataRequest(sale);

            var transactionDataResponse = await PagadorSeachWrapper.GetTransactionData(transactionDataRequest);

            var creditCardResponse = await PagadorSeachWrapper.GetCreditCardData(creditCardRequest);

            var brasPagOrderIdRequest =
                MountBraspagOrderIdDataRequest(sale, transactionDataResponse.BraspagTransactionId);

            var brasPagOrderIdResponse = await PagadorSeachWrapper.GetBraspagOrderId(brasPagOrderIdRequest);

            var searchCustomerRequest = MountCustomerDataRequest(sale, brasPagOrderIdResponse.BraspagOrderId.Value);

            var customerResponse = await  PagadorSeachWrapper.GetCustomerData(searchCustomerRequest);


            responseObject.Payment = MountPaymentResponseObject(transactionDataResponse);
            responseObject.Payment.CreditCard = MountCreditCardResponseObject(creditCardResponse);
            responseObject.Customer = MountCustomerResponseObject(customerResponse);
            
            return responseObject;
        }

        public Customer MountCustomerResponseObject(CustomerDataResponse customerResponse)
        {
            var customer = new Customer()
            {
                Name = customerResponse.CustomerName
            };
            return customer;
        }
        public CreditCard MountCreditCardResponseObject(CreditCardDataResponse creditCardResponse)
        {
            var creditCard = new CreditCard()
            {
                CardNumber = creditCardResponse.CardNumber,
                ExpirationDate = creditCardResponse.CardExpirationDate,
                Holder = creditCardResponse.CardHolder
            };
            return creditCard;
        }
        public Payment MountPaymentResponseObject(TransactionDataResponse transactionDataResponse)
        {
            ProviderEnum provider;
            Enum.TryParse(transactionDataResponse.PaymentMethodName, true, out provider);

            var payment = new Payment()
            {
                Amount = transactionDataResponse.Amount,
                Installments = transactionDataResponse.NumberOfPayments,
                PaymentId = transactionDataResponse.BraspagTransactionId.ToString(),
                Provider = provider

            };
            return payment;
        }


        public TransactionDataRequest MountTransactionDataRequest(ResponseViewModel sale)
        {
            var transactionDataRequest = new TransactionDataRequest()
            {
                BraspagTransactionId = sale.PaymentId,
                MerchantId = new Guid(ConfigurationManager.AppSettings["merchantId"]),
                Version = "1.0",
                RequestId = Guid.NewGuid()
            };

            return transactionDataRequest;
        }
        public CreditCardDataRequest MountCreditCardDataRequest(ResponseViewModel sale)
        {
            var credotCardDataRequest = new CreditCardDataRequest()
            {
                BraspagTransactionId = sale.PaymentId,
                MerchantId = new Guid(ConfigurationManager.AppSettings["merchantId"]),
                Version = "1.0",
                RequestId = Guid.NewGuid()
            };

            return credotCardDataRequest;
        }
        public BraspagOrderIdDataRequest MountBraspagOrderIdDataRequest(ResponseViewModel sale,Guid braspagTransactionId)
        {
            var braspagOrderId = new BraspagOrderIdDataRequest()
            {
                MerchantId = new Guid(ConfigurationManager.AppSettings["merchantId"]),
                Version = "1.0",
                RequestId = Guid.NewGuid(),
                BraspagTransactionId = braspagTransactionId
            };

            return braspagOrderId;
        }
        public CustomerDataRequest MountCustomerDataRequest(ResponseViewModel sale, Guid braspagTransactionId)
        {
            var custumer = new CustomerDataRequest()
            {
                BraspagOrderId = braspagTransactionId,
                MerchantId = new Guid(ConfigurationManager.AppSettings["merchantId"]),
                Version = "1.0",
                RequestId = Guid.NewGuid()
            };

            return custumer;
        }
    }
}