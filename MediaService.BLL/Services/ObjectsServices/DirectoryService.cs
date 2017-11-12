using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MediaService.BLL.Services.ObjectsServices
{
    public class DirectoryService : ObjectsCommonService<DirectoryEntryDto, DirectoryEntry>, IDirectoryService
    {
        public DirectoryService(IUnitOfWork uow) : base(uow)
        {
            Repository = uow.Directories;
        }

        /// <exception cref="InvalidDataException">Thrown when user with given Id doesn't exist in the Database</exception>
        public async Task AddRootDirToUserAsync(string userId)
        {
            var rootDir = new DirectoryEntry
            {
                NodeLevel = 0,
                Created = DateTime.Now,
                Downloaded = DateTime.Now,
                Modified = DateTime.Now,
                Thumbnail = "~/fonts/icons-buttons/folder.svg",
                Name = "root"
            };
            var user = await Database.Users.FindByKeyAsync(userId);
            rootDir.Owner = user ?? throw new InvalidDataException("Can't find user with this Id in database");

            await Repository.AddAsync(rootDir);
            await Database.SaveChangesAsync();
        }
    }
}
