using System;
using FluentAssert;
using IntegracaoPagador.Controllers;
using IntegracaoPagador.Models;
using IntegracaoPagador.Models.Enums;
using IntegracaoPagador.Tests.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using System.Web.Mvc;
using IntegracaoPagador.Service;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace IntegracaoPagador.Tests.Controllers
{
    public class SaleControllerTests
    {
        [Theory, AutoNsubstituteData]
        public void Sut_ShouldGuardItsClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(SaleController).GetConstructors());
        }

        [Theory, AutoNsubstituteData]
        public async void Authorize_WhenModelInvalid_ShouldReturnToSaleView(
            SaleController sut,
            SaleViewModel model)
        {
           sut.ModelState.AddModelError("teste", "deu ruim");

            var act = await sut.Authorize(model);
            var viewResult = act as ViewResult;

            viewResult.ShouldNotBeNull();
            viewResult.Model.ShouldNotBeNull();
            viewResult.Model.ShouldBeEqualTo(model);
            viewResult.ViewName.ShouldBeEqualTo("SaleProduct");

        }

        [Theory, AutoNsubstituteData]
        public async void Authorize_WhenTypeSendSoap_ShouldReturResponseRequest(
            SaleController sut,
            SaleViewModel model,
            ResponseViewModel response)
        {
            model.TypeSend = TypeSendEnum.SOAP;
            sut.SoapRequestService.AuthorizeTransaction(model, Arg.Any<string>()).Returns(response);

            var act = await sut.Authorize(model);
            var viewResult = act as ViewResult;

            viewResult.ShouldNotBeNull();
            viewResult.ViewName.ShouldBeEqualTo("Index");
            viewResult.Model.ShouldNotBeNull();

            (viewResult.Model as ResponseViewModel).TypeSend.ShouldBeEqualTo(TypeSendEnum.REST);

            await sut.SoapRequestService.Received().AuthorizeTransaction(model, Arg.Any<string>());
        }

        [Theory, AutoNsubstituteData]
        public async void Authorize_WhenTypeSendRest_ShouldReturResponseRequest(
            SaleController sut,
            SaleViewModel model,
            ResponseViewModel response)
        {
            model.TypeSend = TypeSendEnum.REST;

            sut.RestRequestService.AuthorizeTransaction(model).Returns(response);

            var act = await sut.Authorize(model);
            var viewResult = act as ViewResult;

            viewResult.ShouldNotBeNull();
            viewResult.ViewName.ShouldBeEqualTo("Index");

            viewResult.Model.ShouldNotBeNull();

            (viewResult.Model as ResponseViewModel).TypeSend.ShouldBeEqualTo(TypeSendEnum.REST);

            await sut.RestRequestService.Received().AuthorizeTransaction(model);

        }

        // Default do enum pegará sempre o SOAP, impossibilitando o teste;
        //[Theory, AutoNsubstituteData]
        //public void Authorize_WhenTypeNotMatch_ShouldReturException(
        //    SaleController sut,
        //    SaleViewModel model)
        //{
        //    model.TypeSend = default(TypeSendEnum);

        //    //var act  = sut.Authorize(model)

        //}

        [Theory, AutoNsubstituteData]
        public async void Authorize_WhenTypeSearchRest_ShouldReturResponseRequest(
            SaleController sut,
           ResponseViewModel model,
           ResponseObject response)
        {
            model.TypeSend = TypeSendEnum.REST;

            sut.SearchRestService.Search(model.PaymentId.ToString()).Returns(response);

            var act = await sut.Search(model);
            var viewResult = act as ViewResult;

            viewResult.ShouldNotBeNull();
            viewResult.ViewName.ShouldBeEqualTo("SearchSale");

            viewResult.Model.ShouldNotBeNull();

            await sut.SearchRestService.Received().Search(model.PaymentId.ToString());

        }
        [Theory, AutoNsubstituteData]
        public async void Authorize_WhenTypeSearchSOAP_ShouldReturResponseRequest(
            SaleController sut,
            ResponseViewModel model,
            ResponseObject response)
        {
            model.TypeSend = TypeSendEnum.SOAP;

            sut.SearchSoapService.Search(model).Returns(response);

            var act = await sut.Search(model);
            var viewResult = act as ViewResult;

            viewResult.ShouldNotBeNull();
            viewResult.ViewName.ShouldBeEqualTo("SearchSale");

            viewResult.Model.ShouldNotBeNull();

            await sut.SearchSoapService.Received().Search(model);

        }
    }
}
