using Coinage.Web.Framework.UI;
using System.Web.Mvc;

namespace Coinage.Web.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Display success alert.
        /// </summary>
        /// <param name="message">Message to display.</param>
        protected virtual void SuccessAlert(string message)
        {
            AddAlert(AlertType.Success, message);
        }

        /// <summary>
        /// Display error alert.
        /// </summary>
        /// <param name="message">Message to display.</param>
        protected virtual void ErrorAlert(string message)
        {
            AddAlert(AlertType.Error, message);
        }

        /// <summary>
        /// Display alert of specified type.
        /// </summary>
        /// <param name="type">Type of alert to display.</param>
        /// <param name="message">Message to display.</param>
        protected virtual void AddAlert(AlertType type, string message)
        {
            TempData[string.Format("coinage.alerts.{0}", type)] = message;
        }
    }
}
