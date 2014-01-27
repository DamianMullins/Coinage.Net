using System;

namespace Coinage.Domain.Concrete
{
    public class EntityActionResponse
    {
        public bool Successful { get; set; }
        public Exception Exception { get; set; }
    }
}
