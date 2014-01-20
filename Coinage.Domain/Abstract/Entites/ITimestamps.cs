using System;

namespace Coinage.Domain.Abstract.Entites
{
    public interface ITimestamps
    {
        DateTime CreatedOn { get; set; }
        DateTime ModifiedOn { get; set; }
    }
}
