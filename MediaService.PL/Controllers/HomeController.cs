#region usings

using AutoMapper;
using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.PL.Models.IdentityModels.Managers;
using MediaService.PL.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.SqlTypes;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MediaService.PL.Utils.Attributes.ErrorHandler;

#endregion

namespace MediaService.PL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        #region MyRegion

        private IUserService _applicationUserService;

        private IDirectoryService _directoryService;

        private IFilesService _filesService;

        private IMapper _mapper;

        private ApplicationUserManager _userManager;

        #endregion

        #region Constructors

        public HomeController() { }

        public HomeController(
            ApplicationUserManager userManager,
            IUserService applicationUserService,
            IDirectoryService directoryService,
            IFilesService filesService
        )
        {
            UserManager = userManager;
            ApplicationUserService = applicationUserService;
            DirectoryService = directoryService;
            FilesService = filesService;
        }

        #endregion

        #region Services Properties

        private IMapper Mapper => _mapper ?? (_mapper = MapperModule.GetMapper());

        private ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            set => _userManager = value;
        }

        private IUserService ApplicationUserService
        {
            get => _applicationUserService ?? HttpContext.GetOwinContext().GetUserManager<IUserService>();
            set => _applicationUserService = value;
        }

        private IDirectoryService DirectoryService
        {
            get => _directoryService ?? HttpContext.GetOwinContext().GetUserManager<IDirectoryService>();
            set => _directoryService = value;
        }

        private IFilesService FilesService
        {
            get => _filesService ?? HttpContext.GetOwinContext().GetUserManager<IFilesService>();
            set => _filesService = value;
        }

        #endregion

        #region Actions
        [ErrorHandle(ExceptionType = typeof(SqlNullValueException), View = "Error")]
        //[HandleErrorAttribute(ExceptionType = typeof(SqlNullValueException), View = "Error")]
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
                throw new SqlNullValueException("We are sorry, but we can't get your data from our's servers at this moment, try again later", ex);
            }
            return View(rootDir);
        }

        public async Task<ActionResult> DirectoriesList(Guid parentId)
        {
            var directories = await DirectoryService.GetByParentIdAsync(parentId);
            return PartialView("_DirectoriesList", directories);
        }

        public async Task<ActionResult> FilesList(Guid parentId)
        {
            var files = await FilesService.GetByParentIdAsync(parentId);
            return PartialView("_FilesList", files);
        }

        #endregion

        #region Overrided Methods

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_directoryService != null)
                {
                    _directoryService.Dispose();
                    _directoryService = null;
                }

                if (_applicationUserService != null)
                {
                    _applicationUserService.Dispose();
                    _applicationUserService = null;
                }

                if (_filesService != null)
                {
                    _filesService.Dispose();
                    _filesService = null;
                }
            }

            base.Dispose(disposing);
        }

        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    filterContext.ExceptionHandled = true;

        //    var handleErrorInfo = new HandleErrorInfo(filterContext.Exception,
        //        filterContext.RouteData.Values["controller"].ToString(),
        //        filterContext.RouteData.Values["action"].ToString());
        //    filterContext.Result = RedirectToAction("Index", "ErrorHandler", handleErrorInfo);
        //}

        #endregion
    }
}