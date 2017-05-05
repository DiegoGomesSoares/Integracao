using System.Threading.Tasks;
using IntegracaoPagador.Models;

namespace IntegracaoPagador.Service
{
    public interface ISoapRequestService
    {
        Task<ResponseRequest> AuthorizeTransactionSoap(SaleViewModel sale, string merchantOrderId);
    }
}