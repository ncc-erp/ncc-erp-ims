using Abp.Authorization;
using Abp.UI;
using Erpinfo.Authorization;
using Erpinfo.Authorization.Users;
using Erpinfo.Constant;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Erpinfo.Entities;
using Erpinfo.Tradition.Dto;
using Microsoft.EntityFrameworkCore;

namespace Erpinfo.Tradition
{
    [AbpAuthorize]

    public class TraditionAlbumAppService : ErpinfoAppServiceBase
    {
        [HttpGet]
        [AbpAuthorize(PermissionNames.TraditionAlbum.GetAllAlbum)]
        public async Task<object> GetAllAlbum()
        {
            return await WorkScope.GetAll<TraditionAlbum>().Select(s => new TraditionAlbumDto
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                Image = s.Image,
                AlbumIndex = s.AlbumIndex,
                AlbumUrl = s.AlbumUrl,
                AlbumTime = s.AlbumTime,
                ParentId = s.ParentId
            }).ToListAsync();
        }
        [HttpGet]
        [AbpAuthorize(PermissionNames.TraditionAlbum.ViewDetailAlbum)]
        public async Task<TraditionAlbumDto> Get(long id)
        {
            return await WorkScope.GetAll<TraditionAlbum>().Where(s => s.Id == id).Select(s => new TraditionAlbumDto
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                Image = s.Image,
                AlbumIndex = s.AlbumIndex,
                AlbumUrl = s.AlbumUrl,
                AlbumTime = s.AlbumTime,
                ParentId = s.ParentId
            }).FirstOrDefaultAsync();
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.TraditionAlbum.Create)]
        public async Task<TraditionAlbumDto> Create(TraditionAlbumDto input)
        {
                TraditionAlbum item = ObjectMapper.Map<TraditionAlbum>(input);
                input.Id = await WorkScope.InsertAndGetIdAsync(item);
                CurrentUnitOfWork.SaveChanges();
            return input;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.TraditionAlbum.Edit)]
        public async Task<TraditionAlbumDto> Update(TraditionAlbumDto input)
        {
                var TraditionAlbum = await WorkScope.GetAsync<TraditionAlbum>(input.Id);
                await WorkScope.UpdateAsync(ObjectMapper.Map<TraditionAlbumDto, TraditionAlbum>(input, TraditionAlbum));
            return input;
        }
        [HttpDelete]
        [AbpAuthorize(PermissionNames.TraditionAlbum.Delete)]
        public async Task Delete(long id)
        {
            var TraditionAlbum = await WorkScope.GetAll<TraditionAlbum>().FirstOrDefaultAsync(s => s.Id == id);
            var HasChild = await WorkScope.GetAll<TraditionAlbum>().AnyAsync(m => m.ParentId == id);
            if (TraditionAlbum == null)
            {
                throw new UserFriendlyException("Album doesn't exist");
            }
            if (HasChild)
            {
                throw new UserFriendlyException("Can't delete album when you have linked album");
            }
            await WorkScope.DeleteAsync<TraditionAlbum>(id);
        }
    }
}
