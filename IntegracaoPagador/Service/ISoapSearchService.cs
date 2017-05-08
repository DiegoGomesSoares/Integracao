using System.Threading.Tasks;
using IntegracaoPagador.Models;

namespace IntegracaoPagador.Service
{
    public interface ISoapSearchService
    {
        Task<ResponseObject> Search(ResponseViewModel sale);
    }
}