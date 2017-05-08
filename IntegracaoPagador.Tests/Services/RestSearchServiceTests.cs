using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FluentAssert;
using IntegracaoPagador.Models;
using IntegracaoPagador.Tests.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Xunit;
using IntegracaoPagador.Service;
using NSubstitute;

namespace IntegracaoPagador.Tests.Services
{
    public class RestSearchServiceTests
    {
        [Theory, AutoNsubstituteData]
        public void Sut_ShouldGuardItsClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(RestSearchService).GetConstructors());
        }

        [Theory, AutoNsubstituteData]
        public void Search_WhenResponseSucess_ShouldReturnObject(
            RestSearchService sut,
            string paymentId,
            ResponseObject response)
        {
            
            var result = sut.Search(paymentId);

            result.ShouldNotBeNull();
           
        }
    }
}
