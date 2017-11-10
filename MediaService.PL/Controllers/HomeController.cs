using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.PL.Models.IdentityModels.Managers;
using MediaService.PL.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NLog;
using MediaService.PL.Models.IdentityModels;

namespace MediaService.PL.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationUserManager _userManager;
        private IApplicationUserService _applicationUserService;
        private IDirectoryService _directoryService;
        private IFilesService _filesService;
        private IMapper _mapper;

        protected static Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        private IMapper Mapper => _mapper ?? (_mapper = MapperModule.GetMapper());

        private ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            set => _userManager = value;
        }

        private IApplicationUserService ApplicationUserService
        {
            get => _applicationUserService ?? HttpContext.GetOwinContext().GetUserManager<IApplicationUserService>();
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

        public HomeController()
        {
        }

        public HomeController(
            ApplicationUserManager userManager,
            IApplicationUserService applicationUserService,
            IDirectoryService directoryService,
            IFilesService filesService
            )
        {
            UserManager = userManager;
            ApplicationUserService = applicationUserService;
            DirectoryService = directoryService;
            FilesService = filesService;
        }

        public async Task<ActionResult> Index(Guid? dirId)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            DirectoryEntryDto rootDir;
            if (dirId.HasValue)
            {
                rootDir = await DirectoryService.FindByIdAsync(dirId.Value);
                return View(rootDir);
            }

            rootDir = (DirectoryService.GetBy(name: "root", owner: Mapper.Map<ApplicationUser, AspNetUserDto>(user))).FirstOrDefault();
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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}