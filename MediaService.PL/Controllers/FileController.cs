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
using System.Web.UI;

#endregion

namespace MediaService.PL.Controllers
{
    public class FileController : Controller
    {
        #region Fields

        private IFileService _filesService;

        private ITagService _tagService;

        private IMapper _mapper;

        #endregion

        #region Constructors

        public FileController() { }

        public FileController(IFileService filesService, ITagService tagService)
        {
            TagService = tagService;
            FilesService = filesService;
        }

        #endregion

        #region Services Properties

        private IMapper Mapper => _mapper ?? (_mapper = MapperModule.GetMapper());

        private IFileService FilesService
        {
            get => _filesService ?? HttpContext.GetOwinContext().GetUserManager<IFileService>();
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
        public async Task<ActionResult> UploadFilesAsync(UploadFilesViewModel model)
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    var filesToUpload = GetFilesToUpload(Request.Files, model.Tags);
                   
                    await FilesService.AddRangeAsync(filesToUpload, model.ParentId);
                    
                    var filesListModel = await FilesService.GetByParentIdAsync(model.ParentId);
                    var html = PartialView("~/Views/File/_FilesList.cshtml", filesListModel).RenderToString();
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

            return PartialView("~/Views/File/_LoadFileFromLink.cshtml");
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

                return PartialView("~/Views/File/_FilesList.cshtml", files);
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

                return PartialView("~/Views/File/_FilesList.cshtml", files);
            }
            catch (Exception e)
            {
                throw new DataException(
                    "We are sorry, but search function is unavaible in this time, try again later", e);
            }
        }

        [HttpGet]
        public ActionResult AddTag(Guid fileId, Guid parentId, string name)
        {
            var model = new AddTagViewModel { FileId = fileId, Name = name, ParentId = parentId };

            return PartialView("~/Views/File/_AddTag.cshtml", model);
        }

        [HttpPost]
        [ErrorHandle(ExceptionType = typeof(DbUpdateException), View = "Errors/Error")]
        public async Task<ActionResult> AddTag(AddTagViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home", new { dirId = model.ParentId });
            }

            try
            {
                await FilesService.AddTagAsync(model.FileId, model.Name);

                return RedirectToAction("Index", "Home", new { dirId = model.ParentId });
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
        public ActionResult Rename(Guid id, Guid parentId, string fileName)
        {
            var model = new RenameFileViewModel { Id = id, ParentId = parentId, Name = fileName};

            return PartialView("~/Views/File/_RenameFile.cshtml", model);
        }

        [HttpPost]
        [ErrorHandle(ExceptionType = typeof(DbUpdateException), View = "Errors/Error")]
        public async Task<ActionResult> Rename(RenameFileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home", new { dirId = model.ParentId });
            }

            try
            {
                if (!await FilesService.ExistAsync(model.Name, model.ParentId))
                {
                    var editedFile = Mapper.Map<FileEntryDto>(model);
                    await FilesService.RenameAsync(editedFile);

                    return RedirectToAction("Index", "Home", new { dirId = model.ParentId });
                }

                ModelState.AddModelError("Name", "The file with this name is already exist in this directory");
            }
            catch (Exception e)
            {
                throw new DbUpdateException(
                    "This file can't be renamed this file at this moment, we're sorry, try again later", e);
            }

            return RedirectToAction("Index", "Home", new { dirId = model.ParentId });
        }

        [HttpGet]
        public ActionResult Delete(Guid fileId, Guid parentId)
        {
            var model = new DeleteFileViewModel { FileId = fileId, ParentId = parentId };

            return PartialView("~/Views/File/_DeleteFile.cshtml", model);
        }

        [HttpPost]
        [ErrorHandle(ExceptionType = typeof(DbUpdateException), View = "Errors/Error")]
        public async Task<ActionResult> Delete(DeleteFileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home", new { dirId = model.ParentId });
            }

            try
            {
                await FilesService.DeleteAsync(model.FileId);

            return RedirectToAction("Index", "Home", new { dirId = model.ParentId });
        }
            catch (Exception e)
            {
                throw new DbUpdateException(
                    "This file can't be deleted this file at this moment, we're sorry, try again later", e);
            }
        }

        [HttpGet]
        [OutputCache(Duration = 300, Location = OutputCacheLocation.Downstream)]
        public async Task<string> GetThumnailLink(Guid fileId)
        {
            var link = await FilesService.GetLinkToFileThumbnailAsync(fileId);
            return link == null
                ? Url.Content("~fonts/icons-buttons/picture-pink.svg")
                : link;
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
                    FileStream = file.InputStream
                };

                if (!string.IsNullOrWhiteSpace(tags[i]))
                {
                    fileEntryDto.Tags = new HashSet<TagDto> { new TagDto {Name = tags[i]} };
                }

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
