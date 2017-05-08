using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using IntegracaoPagador.PagadorSoapSearch;
using IntegracaoPagador.SoapReference;


namespace IntegracaoPagador.Service
{
    public class PagadorSoapSearchClientWrapper : IPagadorSoapSearchClientWrapper
    {
        private readonly PagadorQuerySoapClient _pagadorQuertSoapClient;

        public PagadorSoapSearchClientWrapper()
        {
            this._pagadorQuertSoapClient = new PagadorQuerySoapClient();
        }

        public async Task<PagadorSoapSearch.TransactionDataResponse> GetTransactionData(PagadorSoapSearch.TransactionDataRequest request)
        {
            return  await _pagadorQuertSoapClient.GetTransactionDataAsync(request);
        }

        public async Task<PagadorSoapSearch.CreditCardDataResponse> GetCreditCardData(PagadorSoapSearch.CreditCardDataRequest request)
        {
            return await _pagadorQuertSoapClient.GetCreditCardDataAsync(request);
        }
        public async Task<PagadorSoapSearch.BraspagOrderIdDataResponse> GetBraspagOrderId(PagadorSoapSearch.BraspagOrderIdDataRequest request)
        {
            return await _pagadorQuertSoapClient.GetBraspagOrderIdAsync(request);
        }

        public async Task<PagadorSoapSearch.CustomerDataResponse> GetCustomerData(PagadorSoapSearch.CustomerDataRequest request)
        {
            return await _pagadorQuertSoapClient.GetCustomerDataAsync(request);
        }

    }
}