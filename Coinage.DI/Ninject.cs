using Coinage.Data.EntityFramework;
using Coinage.Data.EntityFramework.Context;
using Coinage.Domain.Abstract.Data;
using Coinage.Domain.Abstract.Services;
using Coinage.Domain.Concrete.Services;
using Ninject;
using Ninject.Web.Common;

namespace Coinage.DI
{
    public static class Ninject
    {
        public static void AddBindings(IKernel kernel)
        {
            // DB Context
            kernel.Bind<IDbContext>().To<CoinageDbContext>();

            // Repository
            kernel.Bind(typeof(IRepository<>)).To(typeof(EfRepository<>)).InRequestScope();

            // Services
            kernel.Bind<IProductService>().To<ProductService>();
        }
    }
}
