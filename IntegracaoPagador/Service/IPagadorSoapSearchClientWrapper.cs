using System.Threading.Tasks;
using IntegracaoPagador.PagadorSoapSearch;

namespace IntegracaoPagador.Service
{
    public interface IPagadorSoapSearchClientWrapper
    {
        Task<BraspagOrderIdDataResponse> GetBraspagOrderId(BraspagOrderIdDataRequest request);
        Task<CreditCardDataResponse> GetCreditCardData(CreditCardDataRequest request);
        Task<CustomerDataResponse> GetCustomerData(CustomerDataRequest request);
        Task<TransactionDataResponse> GetTransactionData(TransactionDataRequest request);
    }
}