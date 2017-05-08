using System;
using Ninject;
using Ninject.Syntax;
using System.Collections.Generic;
using System.Web.Mvc;
using IntegracaoPagador.Service;

namespace IntegracaoPagador.App_Start
{
    public class IocConfig
    {
        public static void DependenciesConfigs()
        {
            IKernel kernel = new StandardKernel();
           
            kernel.Bind<IPagadorSoapClientWrapper>().To<PagadorSoapClientWrapper>();

            kernel.Bind<ISoapRequestService>().To<SoapRequestService>();

            kernel.Bind<IRestRequestService>().To<RestRequestService>();

            kernel.Bind<IRestSearchService>().To<RestSearchService>();

            kernel.Bind<IPagadorSoapSearchClientWrapper>().To<PagadorSoapSearchClientWrapper>();

            kernel.Bind<ISoapSearchService>().To<SoapSearchService>();

            kernel.Bind<IRestSearchServiceWrapper>().To<RestSearchServiceWrapper>();

            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IResolutionRoot _resolutionRoot;

        public NinjectDependencyResolver(IResolutionRoot kernel)
        {
            _resolutionRoot = kernel;
        }

        public object GetService(Type serviceType)
        {
            return _resolutionRoot.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _resolutionRoot.GetAll(serviceType);
        }
    }
}