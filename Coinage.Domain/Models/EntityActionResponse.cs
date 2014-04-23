using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coinage.Domain.Models
{
    /// <summary>
    /// Signifies whether transaction was successful, if not contains an excepyion and/or error message
    /// </summary>
    public class EntityActionResponse
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

        public Exception Exception { get; set; }

        public bool Success { get { return !Errors.Any() && Exception == null; } }

        public EntityActionResponse()
        {
            Errors = new List<string>();
        }
    }
}
