using MediaService.BLL.DTO;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaService.BLL.Services.ObjectsServices
{
    public class ObjectsCommonService<TObjectDto, TObject> : Service<TObjectDto, TObject, Guid>
        where TObjectDto : ObjectEntryDto
        where TObject : ObjectEntry
    {
        public ObjectsCommonService(IUnitOfWork uow) : base(uow) { }

        private TObject RewriteOwners(TObjectDto item)
        {
            var objEntry = DtoMapper.Map<TObject>(item);
            if (objEntry.Owners.Count > 0)
            {
                var owners = new List<AspNetUser>((List<AspNetUser>)objEntry.Owners);
                objEntry.Owners.Clear();
                foreach (var obj in owners)
                {
                    objEntry.Owners.Add(Database.AspNetUsers.FindByKey(obj.Id));
                }
            }

            return objEntry;
        }

        private async Task<TObject> RewriteOwnersAsync(TObjectDto item)
        {
            var objEntry = DtoMapper.Map<TObject>(item);
            if (objEntry.Owners.Count > 0)
            {
                var owners = new List<AspNetUser>((List<AspNetUser>)objEntry.Owners);
                objEntry.Owners.Clear();
                foreach (var obj in owners)
                {
                    objEntry.Owners.Add(await Database.AspNetUsers.FindByKeyAsync(obj.Id));
                }
            }

            return objEntry;
        }

        public override void Add(TObjectDto item)
        {
            Repository.Add(RewriteOwners(item));
            Database.SaveChanges();
        }

        public override async Task AddAsync(TObjectDto item)
        {
            await Repository.AddAsync(await RewriteOwnersAsync(item));
            await Database.SaveChangesAsync();
        }


        public override void AddRange(IEnumerable<TObjectDto> items)
        {
            var objects = DtoMapper.Map<IEnumerable<TObject>>(items);
            Repository.AddRange(objects);
            Database.SaveChanges();
        }

        public override async Task AddRangeAsync(IEnumerable<TObjectDto> items)
        {
            var objects = DtoMapper.Map<IEnumerable<TObject>>(items);
            await Repository.AddRangeAsync(objects);
            await Database.SaveChangesAsync();
        }


        public override void AddRangeParallel(IEnumerable<TObjectDto> items)
        {
            var objects = DtoMapper.Map<IEnumerable<TObject>>(items);
            Repository.AddRangeParallel(objects);
            Database.SaveChanges();
        }

        public override async Task AddRangeAsyncParallel(IEnumerable<TObjectDto> items)
        {
            var objects = DtoMapper.Map<IEnumerable<TObject>>(items);
            await Repository.AddRangeAsyncParallel(objects);
            await Database.SaveChangesAsync();
        }


        public override void Update(TObjectDto item)
        {
            Repository.Update(RewriteOwners(item));
            Database.SaveChanges();
        }

        public override async Task UpdateAsync(TObjectDto item)
        {
            await Repository.UpdateAsync(await RewriteOwnersAsync(item));
            await Database.SaveChangesAsync();
        }

        public IEnumerable<TObjectDto> GetByName(string name)
        {
            return DtoMapper.Map<IEnumerable<TObjectDto>>(Repository.GetDataParallel(o => o.Name.Equals(name)));
        }

        public async Task<IEnumerable<TObjectDto>> GetByNameAsync(string name)
        {
            return DtoMapper.Map<IEnumerable<TObjectDto>>(await Repository.GetDataAsyncParallel(o => o.Name.Equals(name)));
        }

        public IEnumerable<TObjectDto> GetByParentId(Guid id)
        {
            return DtoMapper.Map<IEnumerable<TObjectDto>>(Repository.GetDataParallel(o => o.ParentId.Equals(id)));
        }

        public async Task<IEnumerable<TObjectDto>> GetByParentIdAsync(Guid id)
        {
            return DtoMapper.Map<IEnumerable<TObjectDto>>(await Repository.GetDataAsyncParallel(o => o.ParentId.Equals(id)));
        }

        public IEnumerable<TObjectDto> GetBy(
            Guid? id = null,
            string name = null,
            Guid? parentId = null,
            long? size = null,
            DateTime? created = null,
            DateTime? downloaded = null,
            DateTime? modified = null,
            AspNetUserDto owner = null
            )
        {
            var objects = GetQuery(id, name, parentId, size, created, downloaded, modified, owner);
            return DtoMapper.Map<IEnumerable<TObjectDto>>(objects.AsParallel());
        }

        public async Task<IEnumerable<TObjectDto>> GetByAsync(
            Guid? id = null,
            string name = null,
            Guid? parentId = null,
            long? size = null,
            DateTime? created = null,
            DateTime? downloaded = null,
            DateTime? modified = null,
            AspNetUserDto owner = null
        )
        {
            var objects = GetQuery(id, name, parentId, size, created, downloaded, modified, owner);
            return await Task.Run(() => DtoMapper.Map<IEnumerable<TObjectDto>>(objects.AsParallel()));
        }

        private IQueryable<TObject> GetQuery(
            Guid? id,
            string name,
            Guid? parentId,
            long? size,
            DateTime? created,
            DateTime? downloaded,
            DateTime? modified,
            AspNetUserDto owner
            )
        {
            var objects = Repository.GetQuery();
            if (id.HasValue)
            {
                objects = objects.Intersect(Repository.GetQuery(o => o.Id.Equals(id.Value)));
            }
            if (name != null)
            {
                objects = objects.Intersect(Repository.GetQuery(o => o.Name.Equals(name)));
            }
            if (parentId.HasValue)
            {
                objects = objects.Intersect(Repository.GetQuery(o => o.ParentId.Equals(parentId)));
            }
            if (size.HasValue)
            {
                objects = objects.Intersect(Repository.GetQuery(o => o.Size.Equals(size)));
            }
            if (created.HasValue)
            {
                objects = objects.Intersect(Repository.GetQuery(o => o.Created.Equals(created)));
            }
            if (downloaded.HasValue)
            {
                objects = objects.Intersect(Repository.GetQuery(o => o.Downloaded.Equals(downloaded)));
            }
            if (modified.HasValue)
            {
                objects = objects.Intersect(Repository.GetQuery(o => o.Modified.Equals(modified)));
            }
            if (owner != null)
            {
                var ownerDto = DtoMapper.Map<AspNetUser>(owner);
                objects = objects.Intersect(Repository.GetQuery(o => o.Owners.Contains(ownerDto)));
            }

            return objects;
        }
    }
}
