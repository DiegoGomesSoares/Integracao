using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using IntegracaoPagador.Models;
using IntegracaoPagador.SoapReference;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IntegracaoPagador.Service
{
    public class RestRequestService : IRestRequestService
    {
        const string baseURL = "https://apihomolog.braspag.com.br/v2/sales";

      
        public async Task<ResponseViewModel> AuthorizeTransaction(SaleViewModel sale)
        {
            var restSend = new RestSend
            {
                MerchantOrderId = sale.MerchantOrderId,
                Payment = Mapper.Map<Payment>(sale),
                Customer = Mapper.Map<Customer>(sale),
            };

            restSend.Payment.CreditCard = Mapper.Map<CreditCard>(sale);

            var restSendJsonMessage = JsonConvert.SerializeObject(restSend, new StringEnumConverter());

            var responseRestJson = await SendPostMessage(restSendJsonMessage);

            var responseJsonObject =
                JsonConvert.DeserializeObject<ResponseObject>(responseRestJson);
            var restResponse = new ResponseViewModel
            {
                PaymentId = new Guid(responseJsonObject.Payment.PaymentId)
            };

            return restResponse;
        }

        private static async Task<string> SendPostMessage(string body)
        {
            var client = new HttpClient { BaseAddress = new Uri(baseURL) };

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
            throw new Exception();
        }
    }

}