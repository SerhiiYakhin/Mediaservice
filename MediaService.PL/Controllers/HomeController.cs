#region usings

using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MediaService.BLL.BusinessModels;
using MediaService.BLL.DTO;
using MediaService.BLL.DTO.Enums;
using MediaService.BLL.Interfaces;
using MediaService.PL.Models.IdentityModels.Managers;
using MediaService.PL.Models.ObjectViewModels;
using MediaService.PL.Utils;
using MediaService.PL.Utils.Attributes.ErrorHandler;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

#endregion

namespace MediaService.PL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
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

        #region Fields

        private IUserService _applicationUserService;

        private IDirectoryService _directoryService;

        private IFilesService _filesService;

        private IMapper _mapper;

        private ApplicationUserManager _userManager;

        #endregion

        #region Constructors

        public HomeController()
        {
        }

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

        // Get: /Home/Index
        [HttpGet]
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
                throw new SqlNullValueException(
                    "We are sorry, but we can't get your data from our's servers at this moment, try again later", ex);
            }
            return View(rootDir);
        }

        #region Files Actions

        // POST: /Home/UploadFiles
        [HttpPost]
        [ErrorHandle(ExceptionType = typeof(DbUpdateException), View = "Error")]
        public async Task<ActionResult> UploadFiles(Guid parentId)
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    var filesToUpload = GetFilesToUpload(Request.Files);
                    await FilesService.AddRangeAsync(filesToUpload, parentId);
                }
                catch (Exception ex)
                {
                    throw new DbUpdateException(
                        "New folder can't be added at this moment, we're sorry, try again later", ex);
                }
            }

            //todo: Make return for result form with uploaded files and suggestion for tags addition
            return await FilesList(parentId);
            //return RedirectToAction("FilesList", new { parentId });
        }

        // POST: /Home/FilesList
        [HttpPost]
        public async Task<ActionResult> FilesList(Guid parentId)
        {
            var files = await FilesService.GetByParentIdAsync(parentId);
            return PartialView("_FilesList", files);
        }

        // POST: /Home/DeleteFile
        //[HttpPost]
        //[ErrorHandle(ExceptionType = typeof(DbUpdateException), View = "Error")]
        //public async Task<ActionResult> DeleteFile(Guid fileId)
        //{
        //    try
        //    {
        //        await FilesService.AddAsync(newFolder);
        //        return RedirectToAction("Index", new { parentId = model.ParentId });
        //        ModelState.AddModelError("Name", "The folder with this name is already exist in this directory");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new DbUpdateException("We can't create new folder for you at this moment, we're sorry, try again later", ex);
        //    }

        //    //We get here if were some model validation errors
        //    return PartialView("_CreateFolder", model);
        //}

        #endregion

        #region Directories Actions

        // POST: /Home/DirectoriesList
        [HttpPost]
        public async Task<ActionResult> DirectoriesList(Guid parentId)
        {
            var directories = await DirectoryService.GetByParentIdAsync(parentId);
            return PartialView("_DirectoriesList", directories);
        }

        // POST: /Home/CreateFolder
        [HttpPost]
        [ErrorHandle(ExceptionType = typeof(DbUpdateException), View = "Error")]
        public async Task<ActionResult> CreateFolder(CreateFolderViewModel model)
        {
            try
            {
                if (!await DirectoryService.ExistAsync(model.Name, model.ParentId))
                {
                    var newFolder = Mapper.Map<DirectoryEntryDto>(model);
                    await DirectoryService.AddAsync(newFolder);
                    return RedirectToAction("Index", new { parentId = model.ParentId });
                }
                ModelState.AddModelError("Name", "The folder with this name is already exist in this directory");
            }
            catch (Exception ex)
            {
                throw new DbUpdateException(
                    "We can't create new folder for you at this moment, we're sorry, try again later", ex);
            }

            //We get here if were some model validation errors
            return PartialView("_CreateFolder", model);
        }

        #endregion

        #endregion

        #region Helper Methods

        private static List<FileEntryDto> GetFilesToUpload(HttpFileCollectionBase requestFiles)
        {
            var filesToUpload = new List<FileEntryDto>();
            for (var i = 0; i < requestFiles.Count; i++)
            {
                var file = requestFiles[i];
                var fileType = FileValidation.GetFileTypeValidation(file);
                if (fileType == FileType.Unallowed)
                {
                    //@todo: Add error message about this files to the result json form
                    continue;
                }
                //@todo: Add error message about this files to the result json form
                string fname = Path.GetFileName(file.FileName);
                var fileEntryDto = new FileEntryDto
                {
                    Name = fname,
                    //Created = DateTime.Now,
                    Size = file.ContentLength,
                    FileType = fileType,
                    FileStream = file.InputStream
                };
                filesToUpload.Add(fileEntryDto);
            }

            return filesToUpload;
        }

        #endregion
    }
}