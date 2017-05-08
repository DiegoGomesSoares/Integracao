using System.Net.Http;
using System.Threading.Tasks;

namespace IntegracaoPagador.Service
{
    public interface IRestSearchServiceWrapper
    {
        Task<HttpResponseMessage> GetAsync(string requestUri);
    }
}