using System.Threading.Tasks;
using IntegracaoPagador.Models;

namespace IntegracaoPagador.Service
{
    public interface ISoapRequestService
    {
        Task<ResponseViewModel> AuthorizeTransaction(SaleViewModel sale, string merchantOrderId);
    }
}