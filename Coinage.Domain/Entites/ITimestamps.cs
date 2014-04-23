using System;

namespace Coinage.Domain.Entites
{
    public interface ITimestamps
    {
        DateTime CreatedOn { get; set; }
        DateTime? ModifiedOn { get; set; }
    }
}
