using AutoMapper;
using MediaService.BLL.BusinessModels;
using MediaService.BLL.DTO;
using MediaService.BLL.DTO.Enums;
using MediaService.BLL.Interfaces;
using MediaService.PL.Models.IdentityModels.Managers;
using MediaService.PL.Models.ObjectViewModels.FileViewModels;
using MediaService.PL.Utils;
using MediaService.PL.Utils.Attributes.ErrorHandler;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;
using MediaService.PL.Models.ObjectViewModels.Enums;

namespace MediaService.PL.Controllers
{
    public class FileController : Controller
    {
        #region Fields

        private IUserService _applicationUserService;

        private IDirectoryService _directoryService;

        private IFilesService _filesService;

        private IMapper _mapper;

        private ApplicationUserManager _userManager;

        #endregion

        #region Constructors

        public FileController()
        {
        }

        public FileController(
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

        // POST: /File/Search
        [HttpPost]
        [ErrorHandle(ExceptionType = typeof(DbUpdateException), View = "Errors/Error")]
        public async Task<ActionResult> Search(SearchFilesViewModel model)
        {
            IEnumerable<FileEntryDto> files;
            try
            {
                //todo: Make special methods in FileService and complete this action
                switch (model.SearchType)
                {
                    case SearchType.ByName:
                    //files = await FilesService.GetByAsync(name: model.SearchValue);
                    //break;
                    case SearchType.ByTag:
                    //files = await FilesService.GetByAsync(name: model.SearchValue);
                    //break;
                    default:
                        files = await FilesService.GetByParentIdAsync(model.ParentId);
                        break;
                }
            }
            catch (Exception e)
            {
                throw new DataException("We are sorry, but search function is unavaible in this time, try again later", e);
            }

            switch (model.OrderType)
            {
                case OrderType.BySize:
                    files = files.OrderBy(d => d.Size);
                    break;
                case OrderType.ByName:
                    files = files.OrderBy(d => d.Name);
                    break;
                case OrderType.ByCreationTime:
                    files = files.OrderBy(d => d.Created);
                    break;
                case OrderType.ByUploadingTime:
                    files = files.OrderBy(d => d.Downloaded);
                    break;
            }
            return PartialView("_FilesList", files);
        }

        [HttpGet]
        public ActionResult UploadFiles(Guid folderId)
        {
            var model = new UploadFilesViewModel { ParentId = folderId };

            return PartialView("_UploadFiles", model);
        }

        // POST: /File/UploadFiles
        [HttpPost]
        [ErrorHandle(ExceptionType = typeof(DbUpdateException), View = "Errors/Error")]
        public async Task<ActionResult> UploadFiles(UploadFilesViewModel model)
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    var filesToUpload = GetFilesToUpload(model.Files);
                    await FilesService.AddRangeAsync(filesToUpload, model.ParentId);
                }
                catch (Exception ex)
                {
                    throw new DbUpdateException(
                        "New files can't be uploaded at this moment, we're sorry, try again later", ex);
                }
            }

            //todo: Make return for result form with uploaded files and suggestion for tags addition
            return await FilesList(model.ParentId);
            //return RedirectToAction("FilesList", new { parentId });
        }

        [HttpGet]
        public ActionResult Download(Guid fileId)
        {

            return File(new MemoryStream(), "image/png", "picture.jpg");
        }

        [HttpPost]
        [ErrorHandle(ExceptionType = typeof(DataException), View = "Errors/Error")]
        public ActionResult DownloadFiles(IEnumerable<Guid> filesId)
        {
            try
            {
                // TODO: Add insert logic here
                var zipId = new Guid();
                return Json(new { success = true, zipId}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new DataException(
                    "This files can't be downloaded at this moment, we're sorry, try again later", ex);
            }
        }

        [HttpGet]
        public ActionResult DownloadZip(Guid zipId)
        {
            return File(new MemoryStream(), "application/zip", "FilesFromLux.zip");
        }

        // POST: /File/FilesList
        [HttpPost]
        [ErrorHandle(ExceptionType = typeof(DataException), View = "Errors/Error")]
        public async Task<ActionResult> FilesList(Guid parentId)
        {
            try
            {
                var files = await FilesService.GetByParentIdAsync(parentId);
                return PartialView("_FilesList", files);
            }
            catch (Exception e)
            {
                throw new DataException(
                    "Your files can't be displayed at this moment, we're sorry, try again later", e);
            }
        }

        [HttpGet]
        public ActionResult AddTag(Guid fileId)
        {
            var model = new AddTagViewModel { FileId = fileId };

            return PartialView("_AddTag", model);
        }

        [HttpPost]
        [ErrorHandle(ExceptionType = typeof(DbUpdateException), View = "Errors/Error")]
        public ActionResult AddTag(AddTagViewModel model)
        {
            try
            {
                // TODO: Add insert logic here

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new DbUpdateException(
                    "We can't add tag to this file at this moment, we're sorry, try again later", e);
                //return PartialView("_AddTag", model);
            }
        }

        [HttpGet]
        public ActionResult Rename(Guid fileId, string fileName)
        {
            var model = new RenameFileViewModel { Id = fileId, Name = fileName};
            return PartialView("_RenameFile", model);
        }

        [HttpPost]
        [ErrorHandle(ExceptionType = typeof(DbUpdateException), View = "Errors/Error")]
        public ActionResult Rename(RenameFileViewModel model)
        {
            try
            {
                // TODO: Add insert logic here

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new DbUpdateException(
                    "This file can't be renamed this file at this moment, we're sorry, try again later", e);
                //return PartialView("_RenameFile", model);
            }
        }

        [HttpGet]
        public ActionResult Delete(Guid fileId)
        {
            var model = new DeleteFileViewModel { FileId = fileId };

            return PartialView("_DeleteFile", model);
        }

        [HttpPost]
        [ErrorHandle(ExceptionType = typeof(DbUpdateException), View = "Errors/Error")]
        public ActionResult Delete(DeleteFileViewModel model)
        {
            try
            {
                // TODO: Add insert logic here

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new DbUpdateException(
                    "This file can't be deleted this file at this moment, we're sorry, try again later", e);
                //return PartialView("_DeleteFile", model);
            }
        }

        // POST: /File/DeleteFile
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

        [HttpGet]
        public ActionResult GetThumbnailImg(Stream img)
        {
            return new FileStreamResult(img, "image/png");
        }

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
