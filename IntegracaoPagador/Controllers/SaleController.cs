using System;
using System.Configuration;
using System.Text;
using System.Web.Mvc;
using AutoMapper;
using IntegracaoPagador.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IntegracaoPagador.Models.Enums;
using IntegracaoPagador.PagadorSoapSearch;
using IntegracaoPagador.Service;
using IntegracaoPagador.SoapReference;
using CreditCardDataRequest = IntegracaoPagador.SoapReference.CreditCardDataRequest;
using CustomerDataRequest = IntegracaoPagador.SoapReference.CustomerDataRequest;
using OrderDataRequest = IntegracaoPagador.SoapReference.OrderDataRequest;
using TransactionDataRequest = IntegracaoPagador.PagadorSoapSearch.TransactionDataRequest;

namespace IntegracaoPagador.Controllers
{
    public class SaleController : Controller
    {

        const string baseURL = "https://apihomolog.braspag.com.br/v2/sales";
        const string baseURLSearch = "https://apiqueryhomolog.braspag.com.br/v2/sales/";

        private readonly ISoapRequestService _soapRequestService;
        public SaleController(ISoapRequestService soapService)
        {
            _soapRequestService = soapService;
        }
        // GET: Sale
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SaleProduct()
        {
            return View();
        }

        public async Task<ActionResult> Authorize(SaleViewModel sale)
        {
            if (ModelState.IsValid)
            {
                ResponseRequest responseObject;
                switch (sale.TypeSend)
                {
                    case TypeSendEnum.SOAP:
                        //var soapSendRequestMessage = new AuthorizeTransactionRequest();
                        //soapSendRequestMessage.Version = "v1.0";
                        //soapSendRequestMessage.RequestId = Guid.NewGuid();
                        //soapSendRequestMessage.OrderData = new OrderDataRequest
                        //{
                        //    MerchantId = new Guid(ConfigurationManager.AppSettings["merchantId"]),
                        //    OrderId = CreateMercharOrderId()
                        //};

                        //soapSendRequestMessage.CustomerData = new CustomerDataRequest
                        //{
                        //    CustomerName = sale.CustomerName
                        //};

                        //soapSendRequestMessage.PaymentDataCollection = new PaymentDataRequest[1];
                        //var payment = new CreditCardDataRequest
                        //{
                        //    CardNumber = sale.CreditCardCardNumber,
                        //    Amount = sale.PaymentAmount,
                        //    CardExpirationDate = sale.CreditCardExpirationDate,
                        //    CardHolder = sale.CreditCardHolder,
                        //    CardSecurityCode = sale.CreditCardSecurityCode,
                        //    PaymentMethod = 997,
                        //    Currency = "BRL",
                        //    Country = "BRA",
                        //    NumberOfPayments = (short)(sale.PaymentInstallments),
                        //    PaymentPlan = 0,
                        //    TransactionType = 1
                        //};
                        //soapSendRequestMessage.PaymentDataCollection[0] = payment;

                        ////var soapResponse = new PagadorTransactionSoapClient().AuthorizeTransaction(soapSendRequestMessage);
                        //var soapResponse = await new PagadorTransactionSoapClient().AuthorizeTransactionAsync(soapSendRequestMessage);

                        //responseObject = new ResponseRequest
                        //{
                        //    PaymentId = soapResponse.PaymentDataCollection[0].BraspagTransactionId
                        //};

                        
                        responseObject = await
                            _soapRequestService.AuthorizeTransactionSoap(sale, CreateMercharOrderId());
                        break;
                    case TypeSendEnum.REST:
                        sale.MerchantOrderId = CreateMercharOrderId();

                        var restSend = new RestSend
                        {
                            MerchantOrderId = sale.MerchantOrderId,
                            Payment = Mapper.Map<Payment>(sale),
                            Customer = Mapper.Map<Customer>(sale),
                        };

                        restSend.Payment.CreditCard = Mapper.Map<CreditCard>(sale);

                        var restSendJsonMessage = JsonConvert.SerializeObject(restSend, new StringEnumConverter());

                        var responseRestJson = await SendPostRestMessage(restSendJsonMessage);

                        var responseJsonObject =
                            JsonConvert.DeserializeObject<ResponseRest>(responseRestJson);
                        responseObject = new ResponseRequest
                        {
                            PaymentId = new Guid(responseJsonObject.Payment.PaymentId)
                        };
                        break;
                    default:
                        throw new Exception("Type send not match!");
                }

                return View("Index", responseObject);
            }
            else

            {
                return View("SaleProduct", sale);
            }
        }

        public async Task<ActionResult> Search(ResponseRequest sale)
        {
            var responseObject = new ResponseRest();

            switch (sale.TypeSend)
            {
                case TypeSendEnum.SOAP:

                    var clientSearchTransactionDataMessage = new PagadorSoapSearch.TransactionDataRequest()
                    {
                        BraspagTransactionId = sale.PaymentId,
                        MerchantId = new Guid(ConfigurationManager.AppSettings["merchantId"]),
                        Version = "1.0",
                        RequestId = Guid.NewGuid()
                    };
                   

                    var clientSeachCreditCard = new PagadorSoapSearch.CreditCardDataRequest()
                    {
                        BraspagTransactionId = sale.PaymentId,
                        MerchantId = new Guid(ConfigurationManager.AppSettings["merchantId"]),
                        Version = "1.0",
                        RequestId = Guid.NewGuid()
                    };
                    

                    var soapResponseTransactionData = new PagadorQuerySoapClient().GetTransactionData(clientSearchTransactionDataMessage);

                    var soapResponseCreditCard = new PagadorQuerySoapClient().GetCreditCardData(clientSeachCreditCard);

                    var clientSearchBrasPagOrderId = new BraspagOrderIdDataRequest(){
                      
                        MerchantId = new Guid(ConfigurationManager.AppSettings["merchantId"]),
                        Version = "1.0",
                        RequestId = Guid.NewGuid(),
                        BraspagTransactionId = soapResponseTransactionData.BraspagTransactionId
                    };

                    var soapResponseBrasPagOrderId = new PagadorQuerySoapClient().GetBraspagOrderId(clientSearchBrasPagOrderId);

                    var clientSearchCustomer = new PagadorSoapSearch.CustomerDataRequest()
                    {
                        BraspagOrderId = soapResponseBrasPagOrderId.BraspagOrderId.Value,
                        MerchantId = new Guid(ConfigurationManager.AppSettings["merchantId"]),
                        Version = "1.0",
                        RequestId = Guid.NewGuid()
                    };

                    var soapResponseCustomer =
                        new PagadorQuerySoapClient().GetCustomerData(clientSearchCustomer);

                    ProviderEnum provider;
                     Enum.TryParse(soapResponseTransactionData.PaymentMethodName, true, out provider);

                    responseObject.Payment = new Payment()
                    {
                        Amount = soapResponseTransactionData.Amount,
                        Installments = soapResponseTransactionData.NumberOfPayments,
                        PaymentId = soapResponseTransactionData.BraspagTransactionId.ToString(),
                        Provider = provider
                      
                    };

                    responseObject.Payment.CreditCard = new CreditCard()
                    {
                        CardNumber = soapResponseCreditCard.CardNumber,
                        ExpirationDate = soapResponseCreditCard.CardExpirationDate,
                        Holder = soapResponseCreditCard.CardHolder
                    };
                    
                    responseObject.Customer = new Customer()
                    {
                        Name = soapResponseCustomer.CustomerName
                    };
                    
                    break;
                case TypeSendEnum.REST:
                    var responseRestJson = await SendPostRestSearchMessage(sale.PaymentId.ToString());

                    responseObject =
                        JsonConvert.DeserializeObject<ResponseRest>(responseRestJson, new StringEnumConverter());
                    break;
                default:
                    throw  new Exception("Type send not match!");
            }

            return View("SearchSale", responseObject);
        }

        private async Task<string> SendPostRestSearchMessage(string paymentId)
        {
            var client = new HttpClient {BaseAddress = new Uri(baseURLSearch)};

            var contentType = "application/json";
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

            var merchantId = ConfigurationManager.AppSettings["merchantId"];
            client.DefaultRequestHeaders.Add("MerchantId", merchantId);

            var merchantKey = ConfigurationManager.AppSettings["merchantKey"];
            client.DefaultRequestHeaders.Add("MerchantKey", merchantKey);

         
            var response = await client.GetAsync(paymentId);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                return content;
            }
            else
            {
                throw  new Exception();
            }

        }

        private async Task<string> SendPostRestMessage(string body)
        {

            var client = new HttpClient {BaseAddress = new Uri(baseURL)};

            var contentType = "application/json";

            var merchantId = ConfigurationManager.AppSettings["merchantId"];
            client.DefaultRequestHeaders.Add("MerchantId", merchantId);

            var merchantKey = ConfigurationManager.AppSettings["merchantKey"];
            client.DefaultRequestHeaders.Add("MerchantKey", merchantKey);
            
            var bodySend = new StringContent(body);
            
            bodySend.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        
            var response = await client.PostAsync(baseURL, bodySend);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                return content;
            }
            else
            {
                throw new Exception();
            }
        }

        private string CreateMercharOrderId()
        {
            StringBuilder strbld = new StringBuilder(100);
            Random random = new Random();

            strbld.Append(random.Next());

            return strbld.ToString();
        }
        
    }
}
