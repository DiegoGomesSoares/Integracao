using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using IntegracaoPagador.SoapReference;

namespace IntegracaoPagador.Service
{
    public class PagadorSoapClientWrapper : IPagadorSoapClientWrapper
    {
        private readonly PagadorTransactionSoapClient _pagadorTransactionSoapClient;

        public PagadorSoapClientWrapper()
        {
            _pagadorTransactionSoapClient = new PagadorTransactionSoapClient();
        }

        public Task<AuthorizeTransactionResponse> AuthorizeTransactionAsync(AuthorizeTransactionRequest request)
        {
            return _pagadorTransactionSoapClient.AuthorizeTransactionAsync(request);
        }
    }
}