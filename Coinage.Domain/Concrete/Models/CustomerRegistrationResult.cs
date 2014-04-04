using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coinage.Domain.Concrete.Models
{
    public class CustomerRegistrationResult
    {
        public List<string> Errors { get; set; }

        public string ErrorMessage
        {
            get
            {
                if (Errors.Any())
                {
                    var message = new StringBuilder();

                    foreach (string error in Errors)
                    {
                        message.AppendFormat("{0}, ", error);
                    }

                    return message.ToString().Trim().TrimEnd(',');
                }
                return string.Empty;
            }
        }

        public bool Success { get { return !Errors.Any(); } }

        public CustomerRegistrationResult()
        {
            Errors = new List<string>();
        }
    }
}
