using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.UI;
using Erpinfo.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erpinfo.QuickNew
{
    [AbpAuthorize]
    public class QuickNewsAppService : ErpinfoAppServiceBase, IQuickNewsAppService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QuickNewsAppService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<CreateQuickNewsDto> Create(CreateQuickNewsDto input)
        {
            //if (!CheckSecurityCode())
            //{
            //    throw new UserFriendlyException("IMS server can't connect because of wrong security code");
            //}

            var entity = new QuickNews
            {
                Description = input.Content
            };
            await WorkScope.InsertAsync(entity);

            return input;
        }

        public async Task<UpdateQuickNewsDto> Update(UpdateQuickNewsDto input)
        {
            //if (!CheckSecurityCode())
            //{
            //    throw new UserFriendlyException("IMS server can't connect because of wrong security code");
            //}

            var currentNews = await WorkScope.GetAll<QuickNews>().FirstOrDefaultAsync(n => n.Id == input.Id);
            if (currentNews == null)
            {
                throw new UserFriendlyException("Not found quick news");
            }
            currentNews.Description = input.Content;
            await WorkScope.UpdateAsync(currentNews);
            CurrentUnitOfWork.SaveChanges();
            return input;
        }

        private bool CheckSecurityCode()
        {
            var securityCode = Constant.EntityConstant.SecurityCode;
            var header = _httpContextAccessor.HttpContext.Request.Headers;
            var securityCodeHeader = header["securityCode"];
            if (securityCode == securityCodeHeader)
                return true;
            Logger.Error($"Header SecretCode: {securityCodeHeader.ToString().Substring(0, 3)}");
            Logger.Error($"IMS SecurityCode: {securityCode.ToString().Substring(0, 3)}");
            return false;
        }

        [HttpPost]
        [AbpAuthorize]
        public async Task<PagedResultDto<GetAllQuickNewsDto>> GetAllQuickNews(AllQuickNewsInputDto input)
        {
            var query = WorkScope.GetAll<QuickNews>()
                .Select(s => new GetAllQuickNewsDto
                {
                    Id = s.Id,
                    Description = s.Description,
                    CreationTime = s.CreationTime
                });
            var totalCount = await query.CountAsync();
            var result = await query
               .OrderByDescending(n => n.CreationTime)
               .Skip((input.PageNumber - 1) * input.PageSize).Take(input.PageSize)
               .ToListAsync();
            return new PagedResultDto<GetAllQuickNewsDto>(totalCount, result);
        }

        [HttpPost]
        [AbpAuthorize]
        public async Task<PagedResultDto<GetContentConvertQuickNew>> ChangeFormatTextQuickNews(AllQuickNewsInputDto input)
        {
            var query = WorkScope.GetAll<QuickNews>()
                .Select(s => new GetContentConvertQuickNew
                {
                    Description = s.Description,
                    CreationTime = s.CreationTime
                });

            var totalCount = await query.CountAsync();
            var result = await query
               .OrderByDescending(n => n.CreationTime)
               .Skip((input.PageNumber - 1) * input.PageSize).Take(input.PageSize)
               .ToListAsync();
            return new PagedResultDto<GetContentConvertQuickNew>(totalCount, result);
        }

        public IEnumerable<GetAllQuickNewsDto> GetListQuickNew()
        {
            return WorkScope.GetAll<QuickNews>()
                .Select(s => new GetAllQuickNewsDto
                {
                    Id = s.Id,
                    Description = s.Description,
                    CreationTime = s.CreationTime
                }).ToList();
        }
    }
}