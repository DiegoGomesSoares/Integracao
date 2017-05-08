using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using IntegracaoPagador.SoapReference;
using System.Configuration;
using System.Net.Http.Headers;

namespace IntegracaoPagador.Service
{
    public class RestSearchServiceWrapper : IRestSearchServiceWrapper
    {
        public readonly HttpClient HttpClient;
        const string baseURLSearch = "https://apiqueryhomolog.braspag.com.br/v2/sales/";
        public RestSearchServiceWrapper()
        {
            HttpClient = new HttpClient { BaseAddress = new Uri(baseURLSearch) };
            var contentType = "application/json";
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

            var merchantId = ConfigurationManager.AppSettings["merchantId"];
            HttpClient.DefaultRequestHeaders.Add("MerchantId", merchantId);

            var merchantKey = ConfigurationManager.AppSettings["merchantKey"];
            HttpClient.DefaultRequestHeaders.Add("MerchantKey", merchantKey);
        }





        public Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return HttpClient.GetAsync(requestUri);
        }
    }
}