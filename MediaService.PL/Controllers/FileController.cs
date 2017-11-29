#region usings

using AutoMapper;
using MediaService.BLL.BusinessModels;
using MediaService.BLL.DTO;
using MediaService.BLL.DTO.Enums;
using MediaService.BLL.Interfaces;
using MediaService.BLL.Services.ObjectsServices;
using MediaService.PL.Models.IdentityModels.Managers;
using MediaService.PL.Models.ObjectViewModels.Enums;
using MediaService.PL.Models.ObjectViewModels.FileViewModels;
using MediaService.PL.Utils;
using MediaService.PL.Utils.Attributes.ErrorHandler;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

#endregion

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

        public FileController() { }

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
       
        [HttpPost]
        [ErrorHandle(ExceptionType = typeof(DbUpdateException), View = "Errors/Error")]
        public async Task<ActionResult> Upload(Guid ParentId, List<string> Tags)
        {
           
            if (Request.Files.Count > 0)
            {
                
                try
                {
                    var filesToUpload = GetFilesToUpload(Request.Files, Tags);
                   
                    await FilesService.AddRangeAsync(filesToUpload, ParentId);
                    
                    var FilesListModel = await FilesService.GetByParentIdAsync(ParentId);
                    var html = PartialView("_FilesList", FilesListModel).RenderToString();
                    return Json(new { Success = true, html }, JsonRequestBehavior.AllowGet);
                  
                }
                catch (Exception ex)
                {
                    throw new DbUpdateException(
                        "New files can't be uploaded at this moment, we're sorry, try again later", ex);
                }
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ErrorHandle(ExceptionType = typeof(DbUpdateException), View = "Errors/Error")]
        public async Task<ActionResult> Search(SearchFilesViewModel model)
        {
            try
            {
                var files = await FilesService.SearchFilesAsync(model.ParentId, model.SearchType, model.SearchValue);

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
            catch (Exception e)
            {
                throw new DataException(
                    "We are sorry, but search function is unavaible in this time, try again later", e);
            }
        }

        [HttpGet]
        public ActionResult UploadFiles(Guid folderId)
        {
            var model = new UploadFilesViewModel { ParentId = folderId };

            return PartialView("_UploadFiles", model);
        }

        [HttpGet]
        public async Task<ActionResult> Download(Guid fileId, int fileSize)
        {
            var linkExpirationTime = fileSize / 10;
            var link = await FilesService.GetPublicLinkToFileAsync(fileId, DateTimeOffset.Now.AddMilliseconds(linkExpirationTime));
            
            //return link == null
            //    ? Json(new { success = false }, JsonRequestBehavior.AllowGet)
            //    : Json(new { success = true, link }, JsonRequestBehavior.AllowGet);
            if (link == null)
            {
                return HttpNotFound();
            }

            ViewBag.Link = link;

            return PartialView("_LoadFileRomLink");
        }

        [HttpPost]
        [ErrorHandle(ExceptionType = typeof(DataException), View = "Errors/Error")]
        public async Task<ActionResult> DownloadFiles(DownloadFilesViewModel model)
        {
            try
            {
                var zipId = Guid.NewGuid();
                await FilesService.DownloadWithJobAsync(model.FilesIds, zipId);

                return Json(new { success = true, zipId}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new DataException(
                    "This files can't be downloaded at this moment, we're sorry, try again later", ex);
            }
        }

        [HttpGet]
        public ActionResult DownloadZip(Guid zipId, string zipName)
        {
            var link = FilesService.GetLinkToZip($"{zipId}.zip");

            return link == null
                ? Json(new { success = false }, JsonRequestBehavior.AllowGet)
                : Json(new { success = true, link }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ErrorHandle(ExceptionType = typeof(DataException), View = "Errors/Error")]
        public async Task<ActionResult> FilesList(FilesListViewModel model)
        {
            try
            {
                var files = await FilesService.GetByParentIdAsync(model.ParentId);

                switch (model.OrderType)
                {
                    case OrderType.BySize:
                        files = files.OrderBy(f => f.Size);
                        break;
                    case OrderType.ByName:
                        files = files.OrderBy(f => f.Name);
                        break;
                    case OrderType.ByCreationTime:
                        files = files.OrderBy(f => f.Created);
                        break;
                    case OrderType.ByUploadingTime:
                        files = files.OrderBy(f => f.Downloaded);
                        break;
                }

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
        public async Task<ActionResult> AddTag(AddTagViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_AddTag", model);
            }

            try
            {
                await FilesService.AddTagAsync(model.FileId, model.Name);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new DbUpdateException(
                    "We can't add tag to this file at this moment, we're sorry, try again later", e);
            }
        }

        [HttpGet]
        public ActionResult Rename(Guid fileId, Guid parentId, string fileName)
        {
            var model = new RenameFileViewModel { Id = fileId, ParentId = parentId, Name = fileName};

            return PartialView("_RenameFile", model);
        }

        [HttpPost]
        [ErrorHandle(ExceptionType = typeof(DbUpdateException), View = "Errors/Error")]
        public async Task<ActionResult> Rename(RenameFileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_RenameFile", model);
            }

            try
            {
                if (!await FilesService.ExistAsync(model.Name, model.ParentId))
                {
                    var editedFile = Mapper.Map<FileEntryDto>(model);
                    await FilesService.RenameAsync(editedFile);

                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }

                ModelState.AddModelError("Name", "The file with this name is already exist in this directory");
            }
            catch (Exception e)
            {
                throw new DbUpdateException(
                    "This file can't be renamed this file at this moment, we're sorry, try again later", e);
            }

            return PartialView("_RenameFile", model);
        }

        [HttpGet]
        public ActionResult Delete(Guid fileId)
        {
            var model = new DeleteFileViewModel { FileId = fileId };

            return PartialView("_DeleteFile", model);
        }

        [HttpPost]
        [ErrorHandle(ExceptionType = typeof(DbUpdateException), View = "Errors/Error")]
        public async Task<ActionResult> Delete(DeleteFileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_DeleteFile", model);
            }

            try
            {
                await FilesService.DeleteAsync(model.FileId);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new DbUpdateException(
                    "This file can't be deleted this file at this moment, we're sorry, try again later", e);
            }
        }

        #endregion

        #region Helper Methods

        private static List<FileEntryDto> GetFilesToUpload(HttpFileCollectionBase files, List<string> Tags)
        {
            
            var filesToUpload = new List<FileEntryDto>();
            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                
                var fileType = FileValidation.GetFileTypeValidation(file);
                
               if (fileType == FileType.Unallowed)
                {
                    //@todo: Add error message about this files to the result json form
                    continue;
                }

                string fname = Path.GetFileName(file.FileName);
                var fileTag = Tags[i];
              
                var fileEntryDto = new FileEntryDto
                {
                    Name = fname,
                    Size = file.ContentLength,
                    FileType = fileType,
                    FileStream = file.InputStream,
                    Tags = new HashSet<TagDto> { new TagDto { Name = fileTag } }
                };
                fileEntryDto.Tags.ElementAt(0).FileEntries.Add(fileEntryDto);

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

        #endregion
    }
}
