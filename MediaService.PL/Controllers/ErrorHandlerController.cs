#region usings

using System.Web.Mvc;

#endregion

namespace MediaService.PL.Controllers
{
    public class ErrorHandlerController : Controller
    {
        #region Actions

        // GET: ErrorHandler
        public ActionResult Index()
        {
            Response.StatusCode = 500;
            return View("InternalServerError");
        }

        // GET: Forbidden
        public ActionResult Forbidden()
        {
            Response.StatusCode = 403;
            return View();
        }

        // GET: NotFound
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        #endregion
    }
}