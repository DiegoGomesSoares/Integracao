using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Xunit2;

namespace IntegracaoPagador.Tests.AutoFixture
{
    public class AutoNsubstituteDataAttribute : AutoDataAttribute
    {
        public AutoNsubstituteDataAttribute() 
            :base (new Fixture().Customize(new AutoNSubstituteCustomization())
                                .Customize(new OmitAutoPropertiesCustomization()))
        {
                
        }
        
    }

    public class OmitAutoPropertiesCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<ControllerContext>(c => c
                .Without(x => x.DisplayMode));
        }
    }
}
