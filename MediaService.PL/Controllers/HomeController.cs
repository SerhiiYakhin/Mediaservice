#region usings

using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.PL.Utils.Attributes.ErrorHandler;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

#endregion

namespace MediaService.PL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        #region Fields

        private IDirectoryService _directoryService;

        #endregion

        #region Constructors

        public HomeController() { }

        public HomeController(IDirectoryService directoryService)
        {
            DirectoryService = directoryService;
        }

        #endregion

        #region Services Properties

        private IDirectoryService DirectoryService
        {
            get => _directoryService ?? HttpContext.GetOwinContext().GetUserManager<IDirectoryService>();
            set => _directoryService = value;
        }

        #endregion

        #region Actions

        [HttpGet]
        [ErrorHandle(ExceptionType = typeof(SqlNullValueException), View = "Errors/Error")]
        public async Task<ActionResult> Index(Guid? dirId)
        {
            DirectoryEntryDto rootDir;
            try
            {
                if (dirId.HasValue)
                {
                    rootDir = await DirectoryService.FindByIdAsync(dirId.Value);
                    return View(rootDir);
                }

                var x = User.Identity.GetUserId();
                rootDir = await DirectoryService.GetRootAsync(x);
            }
            catch (Exception ex)
            {
                throw new SqlNullValueException(
                    "We are sorry, but we can't get your data from our's servers at this moment, try again later", ex);
            }

            return View(rootDir);
        }


        #endregion

        #region Overrided Methods

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_directoryService != null)
                {
                    _directoryService.Dispose();
                    _directoryService = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}