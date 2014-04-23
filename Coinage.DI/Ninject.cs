using Coinage.Data.EntityFramework;
using Coinage.Data.EntityFramework.Context;
using Coinage.Domain.Authentication;
using Coinage.Domain.Data;
using Coinage.Domain.Security;
using Coinage.Domain.Services;
using Coinage.Domain.Services.Authentication;
using Coinage.Domain.Services.Security;
using Ninject;
using Ninject.Web.Common;

namespace Coinage.DI
{
    public static class Ninject
    {
        public static void AddBindings(IKernel kernel)
        {
            // DbContext
            kernel.Bind<IDbContext>().To<CoinageDbContext>().InRequestScope();

            // Repositories
            kernel.Bind(typeof(IRepository<>)).To(typeof(EfRepository<>)).InRequestScope();
            kernel.Bind(typeof(IRepositoryAsync<>)).To(typeof(EfRepositoryAsync<>)).InRequestScope();

            // Services
            kernel.Bind<ICustomerService>().To<CustomerService>().InRequestScope();
            kernel.Bind<IProductService>().To<ProductService>().InRequestScope();
            kernel.Bind<IBasketService>().To<BasketService>().InRequestScope();

            kernel.Bind<IAuthenticationService>().To<FormsAuthenticationService>().InRequestScope();

            // Encryption
            kernel.Bind<IEncryptionService>().To<EncryptionService>().InRequestScope();
        }
    }
}
