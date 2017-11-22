#region usings

using System.Web.Mvc;

#endregion

namespace MediaService.PL.Controllers
{
    public class ErrorHandlerController : Controller
    {
        #region Actions

        // GET: ErrorHandler/Forbidden
        public ActionResult Forbidden()
        {
            Response.StatusCode = 403;
            return View();
        }

        // GET: ErrorHandler/NotFound
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        // GET: ErrorHandler/InternalServerError
        public ActionResult InternalServerError()
        {
            Response.StatusCode = 500;
            return View();
        }

        #endregion
    }
}