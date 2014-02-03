using Coinage.Domain.Concrete.Entities;

namespace Coinage.Domain.Abstract
{
    public interface IWorkContext
    {
        Customer CurrentCustomer { get; set; }
    }
}
