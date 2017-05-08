using System;
using System.Configuration;
using System.Text;
using System.Web.Mvc;
using IntegracaoPagador.Models;
using System.Threading.Tasks;
using IntegracaoPagador.Models.Enums;
using IntegracaoPagador.PagadorSoapSearch;
using IntegracaoPagador.Service;
using TransactionDataRequest = IntegracaoPagador.PagadorSoapSearch.TransactionDataRequest;

namespace IntegracaoPagador.Controllers
{
    public class SaleController : Controller
    {
        public readonly ISoapRequestService SoapRequestService;
        public readonly IRestRequestService RestRequestService;
        public readonly IRestSearchService SearchRestService;
        public readonly ISoapSearchService SearchSoapService;
        public readonly IRestSearchServiceWrapper RestSerchServiceWrapper;

        public SaleController(ISoapRequestService soapService, IRestRequestService resquestService,IRestSearchService searchRestSearchService,
            ISoapSearchService searchSoapService, IRestSearchServiceWrapper restSerchServiceWrapper)
        {
            if (soapService == null)
                throw new ArgumentNullException(nameof(soapService));
            if (resquestService == null)
                throw new ArgumentNullException(nameof(resquestService));
            if (searchRestSearchService == null)
                throw new ArgumentNullException(nameof(searchRestSearchService));
            if (searchSoapService == null)
                throw new ArgumentNullException(nameof(searchSoapService));
            if (restSerchServiceWrapper == null)
                throw new ArgumentNullException(nameof(searchSoapService));

            SoapRequestService = soapService;
            RestRequestService = resquestService;
            SearchRestService = searchRestSearchService;
            SearchSoapService = searchSoapService;
            RestSerchServiceWrapper = restSerchServiceWrapper;
        }
        

        // GET: Sale
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SaleProduct()
        {
            return View();
        }

        public async Task<ActionResult> Authorize(SaleViewModel sale)
        {
            if (ModelState.IsValid)
            {
                ResponseViewModel responseObject;
                switch (sale.TypeSend)
                {
                    case TypeSendEnum.SOAP:
                        responseObject = await
                            SoapRequestService.AuthorizeTransaction(sale, CreateMercharOrderId());
                        break;
                    case TypeSendEnum.REST:
                        sale.MerchantOrderId = CreateMercharOrderId();
                        responseObject = await RestRequestService.AuthorizeTransaction(sale);
                        break;
                    default:
                        throw new Exception("Type send not match!");
                }

                return View("Index", responseObject);
            }

            return View("SaleProduct", sale);
            
        }

        public async Task<ActionResult> Search(ResponseViewModel sale)
        {
            var responseObject = new ResponseObject();

            switch (sale.TypeSend)
            {
                case TypeSendEnum.SOAP:
                    
                    responseObject = await SearchSoapService.Search(sale);

                    break;
                case TypeSendEnum.REST:
                    responseObject = await SearchRestService.Search(sale.PaymentId.ToString());
                    break;
                default:
                    throw  new Exception("Type send not match!");
            }

            return View("SearchSale", responseObject);
        }

        public string CreateMercharOrderId()
        {
            StringBuilder strbld = new StringBuilder(100);
            Random random = new Random();

            strbld.Append(random.Next());

            return strbld.ToString();
        }
        
    }
}
