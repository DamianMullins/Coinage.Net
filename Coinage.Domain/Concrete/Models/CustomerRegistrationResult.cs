using System.Collections.Generic;
using System.Linq;

namespace Coinage.Domain.Concrete.Models
{
    public class CustomerRegistrationResult
    {
        public List<string> Errors { get; set; }

        public bool Success { get { return !Errors.Any(); } }

        public CustomerRegistrationResult()
        {
            Errors = new List<string>();
        }
    }
}
