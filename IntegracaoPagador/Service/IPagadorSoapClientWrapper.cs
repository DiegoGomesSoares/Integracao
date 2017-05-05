using System.Threading.Tasks;
using IntegracaoPagador.SoapReference;

namespace IntegracaoPagador.Service
{
    public interface IPagadorSoapClientWrapper
    {
        Task<AuthorizeTransactionResponse> AuthorizeTransactionAsync(AuthorizeTransactionRequest request);
    }
}