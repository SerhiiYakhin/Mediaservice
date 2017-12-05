﻿#region usings

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;

#endregion

namespace MediaService.BLL.Services.ObjectsServices
{
    public class TagService : Service<TagDto, Tag, Guid>, ITagService
    {
        public TagService(IUnitOfWork uow) : base(uow)
        {
            Repository = uow.Tags;
        }

        public TagDto GetTagByName(string name)
        {
            return DtoMapper.Map<TagDto>(Context.Tags.GetQuery(t => t.Name.Equals(name)).SingleOrDefault());
        }

        public async Task<TagDto> GetTagByNameAsync(string name)
        {
            return await Task.Run(() => GetTagByName(name));
        }

        public override void Add(TagDto item)
        {
            var tagEntry = Context.Tags.GetQuery(t => t.Name == item.Name).FirstOrDefault();

            if (tagEntry == null)
            {
                tagEntry = new Tag { Name = item.Name };

                foreach (var fileEntryDto in item.FileEntries)
                {
                    var fileEntry = Context.Files.FindByKey(fileEntryDto.Id);
                    tagEntry.FileEntries.Add(fileEntry);
                }

                Context.Tags.Add(tagEntry);
                Context.SaveChanges();
            }
            else
            {
                throw new InvalidExpressionException(
                    "Tag already exist in database, maybe you wan't to use command Update");
            }
        }

        public override async Task AddAsync(TagDto item)
        {
            await Task.Run(() => Add(item));
        }

        public async Task RemoveAsync(Guid fileId, Guid tagId)
        {
            await Task.Run(() =>
            {
                var currFileEntry = Context.Files.FindByKey(fileId)
                                    ?? throw new InvalidDataException("Can't find user's file with this Id in database"); ;

                var tag = currFileEntry.Tags.FirstOrDefault(t => t.Id == tagId)
                          ?? throw new InvalidDataException("Can't find  file's tag with this Id in database"); ;

                if (tag.FileEntries.Count == 1)
                {
                    Context.Tags.Remove(tag);
                }

                currFileEntry.Tags.Remove(tag);

                Context.Files.Update(currFileEntry);
                Context.SaveChanges();
            });
        }
    }
}