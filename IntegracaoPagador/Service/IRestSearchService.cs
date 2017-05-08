using System.Threading.Tasks;
using IntegracaoPagador.Models;

namespace IntegracaoPagador.Service
{
    public interface IRestSearchService
    {
        Task<ResponseObject> Search(string paymentId);
    }
}