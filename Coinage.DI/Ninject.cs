
using Coinage.Data.EntityFramework;
using Coinage.Data.EntityFramework.Context;
using Coinage.Domain.Abstract;
using Coinage.Domain.Abstract.Authentication;
using Coinage.Domain.Abstract.Data;
using Coinage.Domain.Abstract.Services;
using Coinage.Domain.Concrete.Services;
using Coinage.Domain.Concrete.Services.Authentication;
using Coinage.Web.Framework;
using Ninject;
using Ninject.Web.Common;

namespace Coinage.DI
{
    public static class Ninject
    {
        public static void AddBindings(IKernel kernel)
        {
            // DB Context
            kernel.Bind<IDbContext>().To<CoinageDbContext>().InRequestScope();

            // Repository
            kernel.Bind(typeof(IRepository<>)).To(typeof(EfRepository<>)).InRequestScope();

            // Services
            kernel.Bind<ICustomerService>().To<CustomerService>().InRequestScope();
            kernel.Bind<IProductService>().To<ProductService>().InRequestScope();
            kernel.Bind<IBasketService>().To<BasketService>().InRequestScope();

            kernel.Bind<IAuthenticationService>().To<FormsAuthenticationService>().InRequestScope();
        }
    }
}
