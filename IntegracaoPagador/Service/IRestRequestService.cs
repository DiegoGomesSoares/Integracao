using System.Threading.Tasks;
using IntegracaoPagador.Models;

namespace IntegracaoPagador.Service
{
    public interface IRestRequestService
    {
        Task<ResponseViewModel> AuthorizeTransaction(SaleViewModel sale);
    }
}