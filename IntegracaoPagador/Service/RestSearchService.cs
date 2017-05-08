using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using IntegracaoPagador.Models;
using Newtonsoft.Json.Converters;

namespace IntegracaoPagador.Service
{
    public class RestSearchService : IRestSearchService
    {
        const string baseURLSearch = "https://apiqueryhomolog.braspag.com.br/v2/sales/";
        public IRestSearchServiceWrapper ServiceSearchWrapper;
        public RestSearchService(IRestSearchServiceWrapper searchServiceWrapper)
        {
            ServiceSearchWrapper = searchServiceWrapper;
        }

        public async Task<ResponseObject> Search(string paymentId)
        {
            //var client = new HttpClient { BaseAddress = new Uri(baseURLSearch) };

            //var contentType = "application/json";
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

            //var merchantId = ConfigurationManager.AppSettings["merchantId"];
            //client.DefaultRequestHeaders.Add("MerchantId", merchantId);

            //var merchantKey = ConfigurationManager.AppSettings["merchantKey"];
            //client.DefaultRequestHeaders.Add("MerchantKey", merchantKey);


            var response = await ServiceSearchWrapper.GetAsync(paymentId);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var responseObject = JsonConvert.DeserializeObject<ResponseObject>(content, new StringEnumConverter());

                return responseObject;
            }
            else
            {
                throw new Exception();
            }
        }
    }
}