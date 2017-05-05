using FluentAssert;
using IntegracaoPagador.Controllers;
using IntegracaoPagador.Models;
using IntegracaoPagador.Models.Enums;
using IntegracaoPagador.Tests.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using System.Web.Mvc;
using NSubstitute;
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
            ResponseRequest response)
        {
            model.TypeSend = TypeSendEnum.SOAP;

            sut.SoapRequestService.AuthorizeTransactionSoap(model, sut.CreateMercharOrderId()).Returns(response);

            var act = await sut.Authorize(model);
            var viewResult = act as ViewResult;

            viewResult.ShouldNotBeNull();
            viewResult.ViewName.ShouldBeEqualTo("Index");
            (viewResult.Model as ResponseRequest).ShouldNotBeNull();



        }
    }
}
