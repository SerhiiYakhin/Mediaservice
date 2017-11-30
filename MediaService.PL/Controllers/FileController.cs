#region usings

using AutoMapper;
using MediaService.BLL.BusinessModels;
using MediaService.BLL.DTO;
using MediaService.BLL.DTO.Enums;
using MediaService.BLL.Interfaces;
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

        private IFilesService _filesService;

        private ITagService _tagService;

        private IMapper _mapper;

        #endregion

        #region Constructors

        public FileController() { }

        public FileController(IFilesService filesService, ITagService tagService)
        {
            TagService = tagService;
            FilesService = filesService;
        }

        #endregion

        #region Services Properties

        private IMapper Mapper => _mapper ?? (_mapper = MapperModule.GetMapper());

        private IFilesService FilesService
        {
            get => _filesService ?? HttpContext.GetOwinContext().GetUserManager<IFilesService>();
            set => _filesService = value;
        }

        private ITagService TagService
        {
            get => _tagService ?? HttpContext.GetOwinContext().GetUserManager<ITagService>();
            set => _tagService = value;
        }

        #endregion

        #region Actions

        [HttpGet]
        public ActionResult UploadFiles(Guid folderId)
        {
            var model = new UploadFilesViewModel { ParentId = folderId };

            return PartialView("_UploadFiles", model);
        }

        [HttpPost]
        [ErrorHandle(ExceptionType = typeof(DbUpdateException), View = "Errors/Error")]
        public async Task<ActionResult> UploadFilesAsync(Guid parentId, List<string> tags)
        {
           
            if (Request.Files.Count > 0)
            {
                try
                {
                    var filesToUpload = GetFilesToUpload(Request.Files, tags);
                   
                    await FilesService.AddRangeAsync(filesToUpload, parentId);
                    
                    var filesListModel = await FilesService.GetByParentIdAsync(parentId);
                    var html = PartialView("_FilesList", filesListModel).RenderToString();
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

        [HttpPost]
        [ErrorHandle(ExceptionType = typeof(DbUpdateException), View = "Errors/Error")]
        public async Task<ActionResult> RemoveTag(RemoveTagViewModel model)
        {
            try
            {
                await TagService.RemoveAsync(model.FileId, model.TagId);
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new DbUpdateException(
                    "We can't remove tag from this file at this moment, we're sorry, try again later", e);
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

        private static List<FileEntryDto> GetFilesToUpload(HttpFileCollectionBase files, List<string> tags)
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
              
                var fileEntryDto = new FileEntryDto
                {
                    Name = fname,
                    Size = file.ContentLength,
                    FileType = fileType,
                    FileStream = file.InputStream,
                    Tags = new HashSet<TagDto> { new TagDto { Name = tags[i] } }
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
                if (_filesService != null)
                {
                    _filesService.Dispose();
                    _filesService = null;
                }

                if (_tagService != null)
                {
                    _tagService.Dispose();
                    _tagService = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
